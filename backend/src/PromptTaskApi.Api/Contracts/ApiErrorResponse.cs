namespace PromptTaskApi.Api.Contracts;

public sealed record ApiErrorResponse(
    string Code,
    string Message,
    IReadOnlyList<ApiValidationError>? Errors,
    string TraceId);

public sealed record ApiValidationError(
    string Field,
    string Code,
    string Message);
