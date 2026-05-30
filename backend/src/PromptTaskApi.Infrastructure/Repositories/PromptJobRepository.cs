using System.Data;
using Microsoft.EntityFrameworkCore;
using PromptTaskApi.Application.Abstractions;
using PromptTaskApi.Domain.Prompts;
using PromptTaskApi.Infrastructure.Data;

namespace PromptTaskApi.Infrastructure.Repositories;

internal sealed class PromptJobRepository(PromptTaskDbContext dbContext) : IPromptJobRepository
{
    public async Task AddRangeAsync(IReadOnlyCollection<PromptJob> jobs, CancellationToken cancellationToken)
    {
        await dbContext.PromptJobs.AddRangeAsync(jobs, cancellationToken);
    }

    public async Task<IReadOnlyList<PromptJob>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.PromptJobs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<PromptJob?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.PromptJobs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PromptJob?> ClaimNextProcessableAsync(int maxRetryAttempts, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var job = await dbContext.PromptJobs
            .Where(x => x.Status == PromptStatus.Pending ||
                        (x.Status == PromptStatus.Failed && x.AttemptCount < maxRetryAttempts))
            .OrderBy(x => x.Status == PromptStatus.Pending ? 0 : 1)
            .ThenBy(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return null;
        }

        job.MarkAsProcessing();
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return job;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
