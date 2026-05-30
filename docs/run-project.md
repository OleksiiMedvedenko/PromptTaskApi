# Running PromptTaskSystem

## Requirements

- Docker Desktop
- Docker Compose
- Free ports: `3000`, `5000`, `1433`

## Environment file

Create `.env` in the repository root.

Default mode:

```env
SQL_SERVER_PASSWORD=Your_strong_password123
LANGUAGE_MODEL_PROVIDER=Fake
OPENAI_API_KEY=
OPENAI_MODEL=gpt-4o-mini
```

OpenAI mode:

```env
SQL_SERVER_PASSWORD=Your_strong_password123
LANGUAGE_MODEL_PROVIDER=OpenAI
OPENAI_API_KEY=your-real-openai-token
OPENAI_MODEL=gpt-4o-mini
```

Do not commit the real `.env` file.

## Start

```bash
docker compose up --build
```

## URLs

```txt
Frontend: http://localhost:3000
Swagger:  http://localhost:5000/swagger
Health:   http://localhost:5000/health
```

## Stop

```bash
docker compose down
```

## Clean restart

Use this after changing migrations, NuGet packages, Docker configuration or language model settings.

```bash
docker compose down -v
docker compose build --no-cache
docker compose up
```

## Logs

```bash
docker compose logs -f
docker compose logs -f api
docker compose logs -f worker
```
