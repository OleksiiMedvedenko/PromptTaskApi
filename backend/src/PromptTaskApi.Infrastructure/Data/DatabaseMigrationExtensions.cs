using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PromptTaskApi.Infrastructure.Data;

public static class DatabaseMigrationExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PromptTaskDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PromptTaskDbContext>>();

        logger.LogInformation("Applying database migrations.");
        await dbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Database is ready.");
    }
}
