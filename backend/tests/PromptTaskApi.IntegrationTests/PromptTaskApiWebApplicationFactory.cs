using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PromptTaskApi.Infrastructure.Data;

namespace PromptTaskApi.IntegrationTests;

public sealed class PromptTaskApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // The production app registers SQL Server in the Infrastructure layer.
            // Integration tests replace it with EF Core InMemory, so all provider-specific
            // registrations added by AddDbContext must be removed first.
            var descriptorsToRemove = services
                .Where(descriptor =>
                    descriptor.ServiceType == typeof(DbContextOptions<PromptTaskDbContext>) ||
                    descriptor.ServiceType == typeof(DbContextOptions) ||
                    descriptor.ServiceType.FullName?.Contains("IDbContextOptionsConfiguration") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.RemoveAll<PromptTaskDbContext>();

            services.AddDbContext<PromptTaskDbContext>(options =>
            {
                options.UseInMemoryDatabase($"PromptTaskApiTests-{Guid.NewGuid()}");
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PromptTaskDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }
}
