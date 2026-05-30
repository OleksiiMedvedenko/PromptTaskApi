using Microsoft.Extensions.Options;
using OpenAI.Chat;
using PromptTaskApi.Application.Abstractions;

namespace PromptTaskApi.Infrastructure.LanguageModels;

internal sealed class OpenAiLanguageModelClient(IOptions<OpenAiOptions> options) : ILanguageModelClient
{
    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken)
    {
        var settings = options.Value;
        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.Model))
        {
            throw new InvalidOperationException("OpenAI model is not configured.");
        }

        var client = new ChatClient(model: settings.Model, apiKey: settings.ApiKey);

        ChatMessage[] messages =
        [
            new SystemChatMessage("You are a concise assistant."),
            new UserChatMessage(prompt)
        ];

        ChatCompletion completion = await client.CompleteChatAsync(messages, cancellationToken: cancellationToken);

        return completion.Content.Count > 0
            ? completion.Content[0].Text ?? string.Empty
            : string.Empty;
    }
}
