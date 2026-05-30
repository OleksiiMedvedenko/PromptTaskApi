FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/PromptTaskApi.Worker/PromptTaskApi.Worker.csproj", "src/PromptTaskApi.Worker/"]
COPY ["src/PromptTaskApi.Application/PromptTaskApi.Application.csproj", "src/PromptTaskApi.Application/"]
COPY ["src/PromptTaskApi.Domain/PromptTaskApi.Domain.csproj", "src/PromptTaskApi.Domain/"]
COPY ["src/PromptTaskApi.Infrastructure/PromptTaskApi.Infrastructure.csproj", "src/PromptTaskApi.Infrastructure/"]
RUN dotnet restore "src/PromptTaskApi.Worker/PromptTaskApi.Worker.csproj"
COPY . .
RUN dotnet publish "src/PromptTaskApi.Worker/PromptTaskApi.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PromptTaskApi.Worker.dll"]
