namespace PromptTaskApi.Application.Options;

public sealed class PromptProcessingOptions
{
    public const string SectionName = "PromptProcessing";

    public int PollingIntervalSeconds { get; init; } = 3;
    public int MaxRetryAttempts { get; init; } = 3;
}
