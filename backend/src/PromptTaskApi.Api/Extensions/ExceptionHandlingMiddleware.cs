using FluentValidation;
using PromptTaskApi.Api.Contracts;

namespace PromptTaskApi.Api.Extensions;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private const string ValidationFailedCode = "VALIDATION_FAILED";
    private const string UnexpectedErrorCode = "UNEXPECTED_ERROR";

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(
                ex,
                "Validation failed for request {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            var errors = ex.Errors
                .Select(error => new ApiValidationError(
                    error.PropertyName,
                    string.IsNullOrWhiteSpace(error.ErrorCode) ? ValidationFailedCode : error.ErrorCode,
                    error.ErrorMessage))
                .ToArray();

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ValidationFailedCode,
                "Validation failed.",
                errors);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unhandled API exception for request {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            await WriteErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                UnexpectedErrorCode,
                "Unexpected server error.");
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        int statusCode,
        string code,
        string message,
        IReadOnlyList<ApiValidationError>? errors = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse(
            code,
            message,
            errors,
            context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(response);
    }
}
