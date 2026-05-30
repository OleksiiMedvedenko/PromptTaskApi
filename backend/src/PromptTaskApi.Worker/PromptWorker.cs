using Microsoft.Extensions.Options;
using PromptTaskApi.Application.Options;
using PromptTaskApi.Application.Prompts;

namespace PromptTaskApi.Worker;

public sealed class PromptWorker(
    IServiceScopeFactory scopeFactory,
    IOptions<PromptProcessingOptions> options,
    ILogger<PromptWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Prompt worker started.");

        var delay = TimeSpan.FromSeconds(Math.Max(1, options.Value.PollingIntervalSeconds));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<PromptProcessingService>();

                var processed = await processor.ProcessNextAsync(stoppingToken);
                if (!processed)
                {
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Worker loop failed. Retrying after delay.");
                await Task.Delay(delay, stoppingToken);
            }
        }

        logger.LogInformation("Prompt worker stopped.");
    }
}
