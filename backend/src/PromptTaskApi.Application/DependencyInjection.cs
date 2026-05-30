using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PromptTaskApi.Application.Options;
using PromptTaskApi.Application.Prompts;

namespace PromptTaskApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreatePromptBatchValidator>();
        services.AddOptions<PromptProcessingOptions>()
            .BindConfiguration(PromptProcessingOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<CreatePromptBatchHandler>();
        services.AddScoped<GetPromptJobsHandler>();
        services.AddScoped<PromptProcessingService>();

        return services;
    }
}
