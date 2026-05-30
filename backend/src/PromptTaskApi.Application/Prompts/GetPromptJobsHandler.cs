using PromptTaskApi.Application.Abstractions;

namespace PromptTaskApi.Application.Prompts;

public sealed class GetPromptJobsHandler(IPromptJobRepository repository)
{
    public async Task<IReadOnlyList<PromptJobDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var jobs = await repository.GetAllAsync(cancellationToken);
        return jobs.Select(job => job.ToDto()).ToList();
    }

    public async Task<PromptJobDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var job = await repository.GetByIdAsync(id, cancellationToken);
        return job?.ToDto();
    }
}
