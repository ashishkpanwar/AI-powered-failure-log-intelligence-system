# AI Knowledge Assistant — Copilot Instructions

## What This Project Does
This is an AI-powered failure log intelligence system. It ingests job execution logs, embeds them into Azure Cognitive Search, and exposes a REST API that uses Azure OpenAI to explain failures in natural language.

## Solution Layout (7 projects)

| Project | Purpose |
|---|---|
| `AiKnowledgeAssistant.Domain` | Core models (`JobExecution`, `FailureWindow`) and enums. Zero dependencies. |
| `AiKnowledgeAssistant.Application` | Use-case logic, interfaces, service models, query objects. Depends only on Domain. |
| `AiKnowledgeAssistant.Infrastructure` | EF Core, Azure OpenAI clients, Azure Search vector store. Implements Application interfaces. |
| `AiKnowledgeAssistant.Api` | ASP.NET Core Web API. DI wiring in `CompositionRoot.cs`. |
| `AiKnowledgeAssistant.Ingestion` | Worker that fetches raw logs, normalizes, embeds, and ingests into vector store. |
| `AiKnowledgeAssistant.Tests` | xUnit + Moq integration and unit tests. |
| `Utils` | Shared math helpers (`VectorMath`). |

## Architecture: Clean Architecture

Dependency flow — outer layers depend inward, never the reverse:

```
Domain ← Application ← Infrastructure ← API / Ingestion
```

- **Domain**: no NuGet dependencies, no project references.
- **Application**: defines interfaces for all I/O (AI, vector store, DB). Never references Infrastructure types.
- **Infrastructure**: implements Application interfaces. All Azure SDK usage lives here.
- **API / Ingestion**: entry points that wire up DI and run.

## Coding Conventions

- **Target framework**: `net10.0` across all projects.
- **ImplicitUsings** and **Nullable** are enabled in every project — do not add redundant `using` statements for common BCL types.
- Use `sealed` on classes that are not designed for inheritance.
- Use `required` on properties that must be set at construction and have no meaningful default.
- Use `init` for immutable model properties (records and DTOs).
- Prefer primary constructors for simple services; use full constructor syntax for services with logger injection.
- File names must match the public type they contain exactly — no trailing spaces in filenames.
- One public type per file.

## Key Patterns

**Repository**: `IJobExecutionReader` / `JobExecutionReader` — all DB access goes through repository interfaces, never DbContext directly in Application.

**Strategy**: `ILogNormalizer`, `IFailureVectorStore` — pluggable implementations resolved via DI.

**Builder**: `FailureSummaryBuilder` builds `FailureTechnicalSummary`; prompt building is done via dedicated builder helpers in `Application/Helpers/`.

**Pipeline**: `FailureIngestionPipeline` — fetch → normalize → embed → store.

**Composition Root**: All DI registration lives in `CompositionRoot.cs` (API) or `Program.cs` (Ingestion). Do not scatter `services.Add*` calls across libraries.

**CQRS-influenced**: Query objects live in `Application/Failures/Queries/`. Use sealed record types for queries.

**Resilience**: AI HTTP calls are wrapped with Polly policies from `AiResiliencePolicies`. Always use the policy when calling Azure OpenAI.

## Dependency Injection Rules

- Application-layer interfaces are registered in `CompositionRoot.cs` via `services.AddScoped<IInterface, Implementation>()`.
- Infrastructure registrations go through the `AddInfrastructure()` extension method in `Infrastructure/DependencyInjection.cs`.
- Never call `new ConcreteService(...)` outside of tests.

## Testing Conventions

- Framework: xUnit + Moq.
- Test project `AiKnowledgeAssistant.Tests` uses `TestHostFixture` / `TestHostCollection` for integration test setup.
- Mock Application interfaces (`IAiClient`, `IFailureVectorStore`, etc.); never mock Domain models.
- Unit tests for pure logic (builders, normalizers, guardrails) do not need the test host.
- Test method naming: `MethodName_Scenario_ExpectedOutcome`.

## Azure Services Used

- **Azure OpenAI**: Chat completions (`AzureOpenAiClient`) and embeddings (`AzureOpenAiEmbeddingClient`).
- **Azure Cognitive Search**: Vector store for failure embeddings (`FailureVectorSearchStore`).
- **SQL Server**: Job execution history via EF Core (`JobExecutionDbContext`).

## What NOT To Do

- Do not add business logic to controllers — they delegate to Application services.
- Do not reference Infrastructure types from Application or Domain.
- Do not add raw `HttpClient` usage — use the Azure SDK clients already wired in DI.
- Do not catch exceptions broadly in Application services — let them propagate; controllers handle HTTP mapping.
- Do not skip Polly policies for AI calls.
- Do not register a model class (e.g. `FailureRecord`) as a service implementation in DI.
