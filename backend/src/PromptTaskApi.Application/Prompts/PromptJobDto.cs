using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Application.Prompts;

public sealed record PromptJobDto(
    Guid Id,
    string Prompt,
    PromptStatus Status,
    string? Result,
    string? ErrorMessage,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    DateTimeOffset? ProcessingStartedAtUtc,
    DateTimeOffset? ProcessingFinishedAtUtc,
    int AttemptCount);
