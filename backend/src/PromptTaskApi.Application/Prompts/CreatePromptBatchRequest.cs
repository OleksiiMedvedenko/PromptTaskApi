namespace PromptTaskApi.Application.Prompts;

public sealed record CreatePromptBatchRequest(IReadOnlyCollection<string> Prompts);
