# .NET Clean Architecture Template

A production-ready .NET 10 Web API template based on **Clean Architecture**, with OpenAPI documentation, OpenTelemetry observability, EF Core + SQL Server, and full GitHub Copilot AI support.

---

## Solution structure

```
src/
  Domain/           Pure business rules. No external dependencies.
  Application/      CQRS use cases via MediatR. Abstractions only.
  Infrastructure/   EF Core, SQL Server, external services.
  Api/              Minimal API endpoints. DI composition root.

tests/
  UnitTests/        Fast, isolated tests with NSubstitute mocks.
  IntegrationTests/ Full HTTP round-trips with Testcontainers SQL Server.

.github/
  copilot-instructions.md     Workspace-wide Copilot rules
  prompts/
    new-feature.prompt.md     Scaffold a complete feature end-to-end
    new-entity.prompt.md      Create a domain entity
    new-endpoint.prompt.md    Add a minimal API endpoint
    write-tests.prompt.md     Generate unit and integration tests
    code-review.prompt.md     Architecture + quality checklist
```

---

## Prerequisites

| Tool | Version |
|---|---|
| .NET SDK | 9.x |
| Docker Desktop | Latest |
| SQL Server (via Docker) | 2022 |

---

## Getting started

### 1. Clone and restore

```bash
git clone <your-repo-url>
cd dotnet-ai-template
dotnet restore
```

### 2. Run with Docker Compose

```bash
# Starts API + SQL Server + OpenTelemetry Collector + Jaeger
docker compose up -d
```

- API: http://localhost:8080
- OpenAPI (Scalar UI): http://localhost:8080/scalar/v1
- Jaeger tracing UI: http://localhost:16686

### 3. Run locally (without Docker)

```bash
# Start a SQL Server container only
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Password" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# Apply EF Core migrations
dotnet ef database update -p src/Infrastructure -s src/Api

# Run the API
dotnet run --project src/Api
```

---

## Adding a new feature

Use the Copilot prompt in `.github/prompts/new-feature.prompt.md` or follow these steps manually:

1. **Domain entity** — `src/Domain/{Feature}/{Feature}.cs`
2. **Commands + handlers** — `src/Application/Features/{Feature}/Commands/`
3. **Queries + handlers** — `src/Application/Features/{Feature}/Queries/`
4. **EF configuration** — `src/Infrastructure/Persistence/Configurations/{Feature}Configuration.cs`
5. **Endpoints** — `src/Api/Endpoints/{Feature}Endpoints.cs`
6. **Register in Program.cs** — `app.Map{Feature}Endpoints();`
7. **Add a migration** — `dotnet ef migrations add Add{Feature} -p src/Infrastructure -s src/Api`

---

## EF Core migrations

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> -p src/Infrastructure -s src/Api

# Update the database
dotnet ef database update -p src/Infrastructure -s src/Api

# Revert last migration
dotnet ef migrations remove -p src/Infrastructure -s src/Api
```

---

## Running tests

```bash
# All tests
dotnet test

# Unit tests only (no Docker required)
dotnet test tests/UnitTests

# Integration tests (Docker must be running)
dotnet test tests/IntegrationTests

# With code coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## GitHub Copilot integration

This template is optimized for AI-assisted development:

| File | Purpose |
|---|---|
| `.github/copilot-instructions.md` | Architecture rules applied to every Copilot chat and edit |
| `.github/prompts/new-feature.prompt.md` | Scaffolds a complete feature end-to-end |
| `.github/prompts/new-entity.prompt.md` | Creates a domain aggregate root |
| `.github/prompts/new-endpoint.prompt.md` | Adds a minimal API endpoint |
| `.github/prompts/write-tests.prompt.md` | Generates unit and integration tests |
| `.github/prompts/code-review.prompt.md` | Reviews code for architecture compliance |

### Using prompts

In VS Code with GitHub Copilot Chat:
1. Open any prompt file from `.github/prompts/`
2. Fill in the placeholders (`[FEATURE_NAME]`, etc.)
3. Send to Copilot Chat with **Attach file** or copy-paste the content

---

## Upgrading to .NET 10

1. Install .NET 10 SDK
2. Update `global.json` → `"version": "10.x.xxx"`
3. Update `Directory.Build.props` → `<TargetFramework>net10.0</TargetFramework>`
4. Update package versions in `Directory.Packages.props`

---

## Observability

| Signal | Transport | UI |
|---|---|---|
| Traces | OTLP gRPC (port 4317) | Jaeger at `:16686` |
| Metrics | OTLP gRPC | Coming soon |
| Logs | Console (dev) / OTLP | Coming soon |

Configure the OTLP endpoint via `appsettings.json`:
```json
{
  "OpenTelemetry": {
    "ServiceName": "MyService",
    "OtlpEndpoint": "http://localhost:4317"
  }
}
```

---

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 Minimal APIs |
| Architecture | Clean Architecture + CQRS |
| Mediator | MediatR |
| Validation | FluentValidation |
| ORM | EF Core 9 + SQL Server |
| Observability | OpenTelemetry + Jaeger |
| API Docs | OpenAPI + Scalar |
| Container | Docker + Docker Compose |
| Unit tests | xUnit + NSubstitute + FluentAssertions |
| Integration tests | xUnit + Testcontainers + Microsoft.AspNetCore.Mvc.Testing |
| AI tooling | GitHub Copilot (instructions + reusable prompts) |
