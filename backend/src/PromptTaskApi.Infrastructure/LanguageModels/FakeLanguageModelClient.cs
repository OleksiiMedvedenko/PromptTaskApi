using PromptTaskApi.Application.Abstractions;

namespace PromptTaskApi.Infrastructure.LanguageModels;

internal sealed class FakeLanguageModelClient : ILanguageModelClient
{
    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        return $"Fake model response for prompt: {prompt}";
    }
}
