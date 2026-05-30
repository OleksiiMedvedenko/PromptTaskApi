namespace PromptTaskApi.Application.Abstractions;

public interface ILanguageModelClient
{
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken);
}
