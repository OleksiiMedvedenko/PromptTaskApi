using FluentValidation;

namespace PromptTaskApi.Application.Prompts;

public sealed class CreatePromptBatchValidator : AbstractValidator<CreatePromptBatchRequest>
{
    public CreatePromptBatchValidator()
    {
        RuleFor(x => x.Prompts)
            .NotNull()
            .WithErrorCode("PROMPTS_REQUIRED")
            .WithMessage("Prompts collection is required.")
            .NotEmpty()
            .WithErrorCode("PROMPTS_REQUIRED")
            .WithMessage("At least one prompt is required.");

        RuleFor(x => x.Prompts)
            .Must(prompts => prompts is not null && prompts.Count <= 50)
            .WithErrorCode("PROMPTS_LIMIT_EXCEEDED")
            .WithMessage("A single request can contain up to 50 prompts.");

        RuleForEach(x => x.Prompts)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("PROMPT_REQUIRED")
            .WithMessage("Prompt cannot be empty.")
            .MaximumLength(4_000)
            .WithErrorCode("PROMPT_TOO_LONG")
            .WithMessage("Prompt cannot be longer than 4000 characters.");
    }
}
