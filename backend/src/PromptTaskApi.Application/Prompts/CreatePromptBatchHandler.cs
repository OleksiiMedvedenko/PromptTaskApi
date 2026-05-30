using FluentValidation;
using PromptTaskApi.Application.Abstractions;
using PromptTaskApi.Domain.Prompts;

namespace PromptTaskApi.Application.Prompts;

public sealed class CreatePromptBatchHandler(
    IPromptJobRepository repository,
    IValidator<CreatePromptBatchRequest> validator)
{
    public async Task<IReadOnlyList<PromptJobDto>> HandleAsync(
        CreatePromptBatchRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var jobs = request.Prompts
            .Where(prompt => !string.IsNullOrWhiteSpace(prompt))
            .Select(PromptJob.Create)
            .ToList();

        await repository.AddRangeAsync(jobs, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return jobs.Select(job => job.ToDto()).ToList();
    }
}
