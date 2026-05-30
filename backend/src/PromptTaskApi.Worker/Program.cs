using PromptTaskApi.Application;
using PromptTaskApi.Infrastructure.DependencyInjection;
using PromptTaskApi.Worker;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger, dispose: true);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<PromptWorker>();

var host = builder.Build();
await host.RunAsync();
