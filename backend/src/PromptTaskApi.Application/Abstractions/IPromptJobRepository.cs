using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Application.Abstractions;

public interface IPromptJobRepository
{
    Task AddRangeAsync(IReadOnlyCollection<PromptJob> jobs, CancellationToken cancellationToken);
    Task<IReadOnlyList<PromptJob>> GetAllAsync(CancellationToken cancellationToken);
    Task<PromptJob?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PromptJob?> ClaimNextProcessableAsync(int maxRetryAttempts, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
