namespace PromptTaskApi.Api.Contracts;

public sealed record CreatePromptsHttpRequest(IReadOnlyCollection<string> Prompts);
