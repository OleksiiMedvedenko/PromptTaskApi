using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PromptTaskApi.Application.Abstractions;
using PromptTaskApi.Application.Options;

namespace PromptTaskApi.Application.Prompts;

public sealed class PromptProcessingService(
    IPromptJobRepository repository,
    ILanguageModelClient languageModelClient,
    IOptions<PromptProcessingOptions> options,
    ILogger<PromptProcessingService> logger)
{
    public async Task<bool> ProcessNextAsync(CancellationToken cancellationToken)
    {
        var maxRetryAttempts = Math.Max(1, options.Value.MaxRetryAttempts);
        var job = await repository.ClaimNextProcessableAsync(maxRetryAttempts, cancellationToken);
        if (job is null)
        {
            return false;
        }

        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["PromptJobId"] = job.Id
        });

        try
        {
            logger.LogInformation("Prompt job processing started. Attempt {AttemptCount} of {MaxRetryAttempts}.",
                job.AttemptCount,
                maxRetryAttempts);

            var result = await languageModelClient.GenerateAsync(job.Prompt, cancellationToken);

            job.Complete(result);
            await repository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Prompt job processing completed.");
            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Prompt job processing failed.");

            try
            {
                job.Fail(ex.Message);
                await repository.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception saveException)
            {
                logger.LogError(saveException, "Failed to persist prompt job failure state.");
            }

            return true;
        }
    }
}
