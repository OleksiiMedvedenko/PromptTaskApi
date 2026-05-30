# PromptTaskSystem

PromptTaskSystem is a fullstack application for submitting multiple prompts, processing them in the background, and checking their current status and result.

The system contains:

- Backend API: ASP.NET Core Web API (.NET 9)
- Worker: separate .NET background process
- Database: SQL Server 2022
- Frontend: React 19, TypeScript, Vite
- Runtime: Docker Compose

By default, the project uses a fake language model provider, so it can be started without any external API key. OpenAI can be enabled through environment variables.

---

## Features

- Submit many prompts in one request
- Store every prompt in SQL Server
- Process prompts asynchronously in a separate Worker process
- Track prompt status: `Pending`, `Processing`, `Completed`, `Failed`
- Retry failed prompt jobs up to the configured attempt limit
- Show prompt results and errors in the UI
- Refresh statuses using polling
- Switch UI language between Polish and English
- Run the whole system with one Docker Compose command
- Use Fake or OpenAI language model provider

---

## Repository structure

```txt
PromptTaskSystem/
  backend/
    src/
      PromptTaskApi.Api/
      PromptTaskApi.Application/
      PromptTaskApi.Domain/
      PromptTaskApi.Infrastructure/
      PromptTaskApi.Worker/
    tests/
      PromptTaskApi.UnitTests/
      PromptTaskApi.IntegrationTests/
    PromptTaskApi.sln

  frontend/
    src/
      api/
      features/
      i18n/
      shared/
      styles/
    Dockerfile
    nginx.conf
    package.json
    package-lock.json

  docs/
    PromptTaskSystem_Backend_Documentation.docx
    PromptTaskSystem_Frontend_Documentation.docx
    PromptTaskSystem_Run_Guide.docx
    run-project.md

  docker-compose.yml
  .env.example
  README.md
```

---

## Documentation files

| File | Description |
| --- | --- |
| `docs/PromptTaskSystem_Backend_Documentation.docx` | Backend structure, API, Application, Domain, Infrastructure, Worker, database and OpenAI SDK integration |
| `docs/PromptTaskSystem_Frontend_Documentation.docx` | Frontend structure, prompt form, prompt list, polling, translations and API communication |
| `docs/PromptTaskSystem_Run_Guide.docx` | Docker Compose run guide, environment variables, OpenAI setup and useful commands |
| `docs/run-project.md` | Short markdown run instructions |

---

## System flow

```txt
React Frontend
      |
      v
ASP.NET Core API
      |
      v
SQL Server
      ^
      |
.NET Worker
      |
      v
Language Model Provider
```

1. User creates a batch of prompts in the frontend.
2. Frontend sends the prompts to the API.
3. API validates the request and saves prompt jobs in SQL Server with status `Pending`.
4. Worker claims the next processable job and changes its status to `Processing`.
5. Worker calls the configured language model provider.
6. Worker saves the result and marks the job as `Completed`.
7. If processing fails, Worker saves the error and marks the job as `Failed`.
8. Failed jobs can be retried while `AttemptCount` is lower than `PromptProcessing:MaxRetryAttempts`.
9. Frontend polls the API and shows updated statuses and results.

---

## Requirements

Install:

- Docker Desktop
- Docker Compose

Required free ports:

```txt
3000 - frontend
5000 - API
1433 - SQL Server
```

---

## Environment configuration

Create a real `.env` file in the repository root. You can copy it from `.env.example`.

Default configuration:

```env
SQL_SERVER_PASSWORD=Your_strong_password123
LANGUAGE_MODEL_PROVIDER=Fake
OPENAI_API_KEY=
OPENAI_MODEL=gpt-4o-mini
```

OpenAI configuration:

```env
SQL_SERVER_PASSWORD=Your_strong_password123
LANGUAGE_MODEL_PROVIDER=OpenAI
OPENAI_API_KEY=your-real-openai-token
OPENAI_MODEL=gpt-4o-mini
```

Do not commit the real `.env` file. It is ignored by `.gitignore`.

When running through Docker Compose, values from `.env` are passed to containers as environment variables. These values override matching values from `appsettings.json`.

Example mapping:

```txt
LANGUAGE_MODEL_PROVIDER -> LanguageModel__Provider -> LanguageModel:Provider
OPENAI_API_KEY          -> OpenAI__ApiKey          -> OpenAI:ApiKey
OPENAI_MODEL            -> OpenAI__Model           -> OpenAI:Model
```

The Worker is the component that calls the language model.

---

## Run with Docker Compose

From the repository root:

```bash
docker compose up --build
```

Available URLs:

```txt
Frontend: http://localhost:3000
Swagger:  http://localhost:5000/swagger
Health:   http://localhost:5000/health
SQL:      localhost,1433
```

Stop containers:

```bash
docker compose down
```

Stop containers and remove SQL Server data volume:

```bash
docker compose down -v
```

Use `down -v` when you want to recreate the database from scratch.

After changing packages, migrations, Dockerfiles or environment variables, a clean rebuild can be useful:

```bash
docker compose down -v
docker compose build --no-cache
docker compose up
```

---

## API endpoints

### Create prompt jobs

```http
POST /api/prompts/batch
```

Request:

```json
{
  "prompts": [
    "Explain Clean Architecture in simple words",
    "Generate 3 ideas for a React dashboard"
  ]
}
```

Response: `202 Accepted`

```json
[
  {
    "id": "b2c5a9e2-6d8a-4af2-bd9c-9c7b51eaf000",
    "prompt": "Explain Clean Architecture in simple words",
    "status": "Pending",
    "result": null,
    "errorMessage": null,
    "attemptCount": 0,
    "createdAtUtc": "2026-05-28T16:00:00Z",
    "processingStartedAtUtc": null,
    "processingFinishedAtUtc": null,
    "updatedAtUtc": null
  }
]
```

### Get all prompt jobs

```http
GET /api/prompts
```

Returns prompt jobs ordered by creation date descending.

### Get prompt job by id

```http
GET /api/prompts/{id}
```

Returns one prompt job or `404 Not Found`.

---

## Prompt statuses

| Status | Description |
| --- | --- |
| `Pending` | Prompt job was created and waits for processing |
| `Processing` | Worker claimed the job and is processing it |
| `Completed` | Processing finished successfully and result was saved |
| `Failed` | Processing failed and error message was saved |

Worker retries failed jobs while `AttemptCount` is lower than `PromptProcessing:MaxRetryAttempts`.

Default retry configuration:

```json
"PromptProcessing": {
  "PollingIntervalSeconds": 3,
  "MaxRetryAttempts": 3
}
```

---

## Error response format

The API returns stable error codes. The frontend uses these codes to show translated messages.

Example:

```json
{
  "code": "VALIDATION_FAILED",
  "message": "Validation failed.",
  "errors": [
    {
      "field": "Prompts",
      "code": "PROMPTS_REQUIRED",
      "message": "At least one prompt is required."
    }
  ],
  "traceId": "0HNLSQK24QF61:00000007"
}
```

Known error codes:

```txt
VALIDATION_FAILED
PROMPTS_REQUIRED
PROMPTS_LIMIT_EXCEEDED
PROMPT_REQUIRED
PROMPT_TOO_LONG
PROMPT_NOT_FOUND
UNEXPECTED_ERROR
```

---

## Backend

Backend projects:

```txt
PromptTaskApi.Api
PromptTaskApi.Application
PromptTaskApi.Domain
PromptTaskApi.Infrastructure
PromptTaskApi.Worker
```

### Api

Contains HTTP controllers, Swagger configuration, CORS, health checks and error middleware.

Main endpoints:

```txt
POST /api/prompts/batch
GET  /api/prompts
GET  /api/prompts/{id}
GET  /health
```

### Application

Contains use cases, validators, DTOs and abstractions.

Main responsibilities:

- creating prompt batches
- reading prompt jobs
- processing one job
- validating input
- mapping domain entities to DTOs

### Domain

Contains the `PromptJob` entity and `PromptStatus` enum.

`PromptJob` controls its own state transitions:

```txt
Pending -> Processing -> Completed
Pending -> Processing -> Failed
Failed  -> Processing -> Completed
Failed  -> Processing -> Failed
```

### Infrastructure

Contains EF Core, SQL Server configuration, repository implementation and language model clients.

Implemented providers:

```txt
FakeLanguageModelClient
OpenAiLanguageModelClient
```

`OpenAiLanguageModelClient` uses the official `OpenAI` .NET SDK package behind the local `ILanguageModelClient` abstraction. The package is pinned to `OpenAI` 2.1.0 to keep the .NET 9 dependency graph stable.

### Worker

Runs as a separate process. It periodically checks for processable jobs, claims one job, calls the configured language model provider and saves the result.

The Worker claims jobs inside a serializable database transaction.

---

## Database

The project uses SQL Server and EF Core.

Main table:

```txt
PromptJobs
```

Important fields:

```txt
Id
Prompt
Status
Result
ErrorMessage
AttemptCount
CreatedAtUtc
ProcessingStartedAtUtc
ProcessingFinishedAtUtc
UpdatedAtUtc
RowVersion
```

The API applies EF Core migrations on startup.

---

## Frontend

Frontend is built with React 19, TypeScript and Vite.

Main responsibilities:

- create a batch of prompts
- submit prompts to the API
- show prompt list
- show statuses, results and errors
- refresh data by polling
- support Polish and English UI

Main frontend areas:

```txt
api/                 API client and error helpers
features/prompts/    prompt form, list, cards, polling logic
i18n/                translations and language handling
shared/              shared UI helpers/components
styles/              global styles
```

The polling logic skips a new request if the previous request is still running. Active requests are aborted when the component unmounts.

---

## Language model providers

### Fake provider

Default provider:

```env
LANGUAGE_MODEL_PROVIDER=Fake
```

It returns a fake response. No API key is required.

### OpenAI provider

OpenAI provider:

```env
LANGUAGE_MODEL_PROVIDER=OpenAI
OPENAI_API_KEY=your-real-openai-token
OPENAI_MODEL=gpt-4o-mini
```

The implementation uses the official `OpenAI` NuGet package and is hidden behind `ILanguageModelClient`.

---

## Development mode

Start only SQL Server:

```bash
docker compose up -d sqlserver
```

Run API locally:

```bash
cd backend
dotnet run --project src/PromptTaskApi.Api
```

Run Worker locally in another terminal:

```bash
cd backend
dotnet run --project src/PromptTaskApi.Worker
```

Run frontend locally:

```bash
cd frontend
npm install
npm run dev
```

Frontend local URL:

```txt
http://localhost:5173
```

---

## Tests

Run backend tests:

```bash
cd backend
dotnet test
```

The solution contains:

- unit tests
- integration tests

---

## Docker Compose services

| Service | Description | Public port |
| --- | --- | --- |
| `sqlserver` | SQL Server database | `1433` |
| `api` | ASP.NET Core Web API | `5000` |
| `worker` | background prompt processor | none |
| `frontend` | React app served by nginx | `3000` |

Inside Docker, API and Worker connect to SQL Server using:

```txt
Server=sqlserver,1433
```

When running API or Worker locally against SQL Server from Docker, use:

```txt
Server=localhost,1433
```

---

## Useful commands

Rebuild and start everything:

```bash
docker compose up --build
```

Clean rebuild with fresh database:

```bash
docker compose down -v
docker compose build --no-cache
docker compose up
```

Show logs:

```bash
docker compose logs -f
```

Show Worker logs:

```bash
docker compose logs -f worker
```

Show API logs:

```bash
docker compose logs -f api
```

Run frontend build locally:

```bash
cd frontend
npm ci
npm run build
```

Run backend tests:

```bash
cd backend
dotnet test
```
