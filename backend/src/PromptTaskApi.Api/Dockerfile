FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/PromptTaskApi.Api/PromptTaskApi.Api.csproj", "src/PromptTaskApi.Api/"]
COPY ["src/PromptTaskApi.Application/PromptTaskApi.Application.csproj", "src/PromptTaskApi.Application/"]
COPY ["src/PromptTaskApi.Domain/PromptTaskApi.Domain.csproj", "src/PromptTaskApi.Domain/"]
COPY ["src/PromptTaskApi.Infrastructure/PromptTaskApi.Infrastructure.csproj", "src/PromptTaskApi.Infrastructure/"]
RUN dotnet restore "src/PromptTaskApi.Api/PromptTaskApi.Api.csproj"
COPY . .
RUN dotnet publish "src/PromptTaskApi.Api/PromptTaskApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PromptTaskApi.Api.dll"]
