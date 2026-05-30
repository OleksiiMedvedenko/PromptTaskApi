using Microsoft.EntityFrameworkCore;
using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Infrastructure.Data;

public sealed class PromptTaskDbContext(DbContextOptions<PromptTaskDbContext> options) : DbContext(options)
{
    public DbSet<PromptJob> PromptJobs => Set<PromptJob>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PromptTaskDbContext).Assembly);
    }
}
