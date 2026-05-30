using PromptTaskApi.Domain.Common;

namespace PromptTaskApi.Domain.Prompts;

public sealed class PromptJob : Entity
{
    private PromptJob() { }

    private PromptJob(string prompt)
    {
        Prompt = prompt;
        Status = PromptStatus.Pending;
    }

    public string Prompt { get; private set; } = string.Empty;
    public PromptStatus Status { get; private set; }
    public string? Result { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTimeOffset? ProcessingStartedAtUtc { get; private set; }
    public DateTimeOffset? ProcessingFinishedAtUtc { get; private set; }
    public int AttemptCount { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public static PromptJob Create(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new ArgumentException("Prompt cannot be empty.", nameof(prompt));
        }

        return new PromptJob(prompt.Trim());
    }

    public void MarkAsProcessing()
    {
        if (Status is not PromptStatus.Pending and not PromptStatus.Failed)
        {
            throw new InvalidOperationException($"Prompt job cannot be processed from status '{Status}'.");
        }

        Status = PromptStatus.Processing;
        AttemptCount++;
        ErrorMessage = null;
        ProcessingStartedAtUtc = DateTimeOffset.UtcNow;
        ProcessingFinishedAtUtc = null;
        MarkUpdated();
    }

    public void Complete(string result)
    {
        if (Status != PromptStatus.Processing)
        {
            throw new InvalidOperationException("Only processing jobs can be completed.");
        }

        Status = PromptStatus.Completed;
        Result = result;
        ErrorMessage = null;
        ProcessingFinishedAtUtc = DateTimeOffset.UtcNow;
        MarkUpdated();
    }

    public void Fail(string errorMessage)
    {
        if (Status != PromptStatus.Processing)
        {
            throw new InvalidOperationException("Only processing jobs can fail.");
        }

        Status = PromptStatus.Failed;
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown processing error." : errorMessage.Trim();
        ProcessingFinishedAtUtc = DateTimeOffset.UtcNow;
        MarkUpdated();
    }
}
