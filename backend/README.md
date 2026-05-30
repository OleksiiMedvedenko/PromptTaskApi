# PromptTaskApi backend

This folder contains the backend part of PromptTaskSystem.

## Projects

```txt
src/PromptTaskApi.Api/             ASP.NET Core Web API
src/PromptTaskApi.Application/     use cases, validation, DTOs, abstractions
src/PromptTaskApi.Domain/          domain entities and statuses
src/PromptTaskApi.Infrastructure/  EF Core, repositories, LLM clients
src/PromptTaskApi.Worker/          background processing process
tests/PromptTaskApi.UnitTests/
tests/PromptTaskApi.IntegrationTests/
```

## Run backend tests

```bash
dotnet test
```

## Development mode

From the repository root start SQL Server:

```bash
docker compose up -d sqlserver
```

Then from this folder run API and Worker separately:

```bash
dotnet run --project src/PromptTaskApi.Api
```

```bash
dotnet run --project src/PromptTaskApi.Worker
```

The full system is started from the repository root with:

```bash
docker compose up --build
```
