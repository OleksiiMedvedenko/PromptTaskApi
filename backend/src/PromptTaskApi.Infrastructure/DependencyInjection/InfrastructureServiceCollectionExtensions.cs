using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromptTaskApi.Application.Abstractions;
using PromptTaskApi.Infrastructure.Data;
using PromptTaskApi.Infrastructure.LanguageModels;
using PromptTaskApi.Infrastructure.Repositories;

namespace PromptTaskApi.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PromptTaskDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IPromptJobRepository, PromptJobRepository>();

        var provider = configuration.GetValue<string>("LanguageModel:Provider") ?? "Fake";
        if (provider.Equals("OpenAI", StringComparison.OrdinalIgnoreCase))
        {
            services.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SectionName));
            services.AddScoped<ILanguageModelClient, OpenAiLanguageModelClient>();
        }
        else
        {
            services.AddScoped<ILanguageModelClient, FakeLanguageModelClient>();
        }

        return services;
    }
}
