using Microsoft.AspNetCore.Mvc;
using PromptTaskApi.Api.Contracts;
using PromptTaskApi.Application.Prompts;

namespace PromptTaskApi.Api.Controllers;

[ApiController]
[Route("api/prompts")]
[Produces("application/json")]
public sealed class PromptsController(
    CreatePromptBatchHandler createPromptBatchHandler,
    GetPromptJobsHandler getPromptJobsHandler) : ControllerBase
{
    private const string PromptNotFoundCode = "PROMPT_NOT_FOUND";

    /// <summary>
    /// Creates multiple prompt processing jobs.
    /// </summary>
    /// <remarks>
    /// Jobs are stored as Pending and processed asynchronously by the Worker process.
    /// </remarks>
    [HttpPost("batch")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<PromptJobDto>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBatch(
        [FromBody] CreatePromptsHttpRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createPromptBatchHandler.HandleAsync(
            new CreatePromptBatchRequest(request.Prompts),
            cancellationToken);

        return Accepted(result);
    }

    /// <summary>
    /// Returns all prompt jobs with their current statuses and processing results.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PromptJobDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await getPromptJobsHandler.HandleAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns a single prompt job by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PromptJobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await getPromptJobsHandler.HandleAsync(id, cancellationToken);

        if (result is not null)
        {
            return Ok(result);
        }

        return NotFound(new ApiErrorResponse(
            PromptNotFoundCode,
            "Prompt job was not found.",
            null,
            HttpContext.TraceIdentifier));
    }
}
