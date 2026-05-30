using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Application.Prompts;

internal static class PromptJobMapper
{
    public static PromptJobDto ToDto(this PromptJob job) => new(
        job.Id,
        job.Prompt,
        job.Status,
        job.Result,
        job.ErrorMessage,
        job.CreatedAtUtc,
        job.UpdatedAtUtc,
        job.ProcessingStartedAtUtc,
        job.ProcessingFinishedAtUtc,
        job.AttemptCount);
}
