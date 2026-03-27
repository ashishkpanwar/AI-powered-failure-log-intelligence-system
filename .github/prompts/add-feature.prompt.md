---
mode: agent
description: Scaffold a new feature following Clean Architecture conventions for this project
---

Add the following feature to the AI Knowledge Assistant system:

**Feature**: ${input:featureDescription}

Follow these steps and conventions exactly:

### 1. Domain (if new entities/enums are needed)
- Add to `AiKnowledgeAssistant.Domain/Models/` or `AiKnowledgeAssistant.Domain/Enums/`
- No external dependencies — pure C# only
- Use `sealed` class with `init` properties

### 2. Application Layer
- Define the interface in `AiKnowledgeAssistant.Application/` under the relevant feature folder
- Add a query object in `Queries/` as a `sealed record` if this is a read operation
- Add the use-case service implementation in `Implementations/`
- The service must depend only on interfaces, never on Infrastructure types

### 3. Infrastructure Layer (if I/O is needed)
- Implement the Application interface in `AiKnowledgeAssistant.Infrastructure/`
- Register it in `Infrastructure/DependencyInjection.cs` via the `AddInfrastructure()` extension

### 4. API Layer
- Add a controller in `AiKnowledgeAssistant.Api/Controllers/`
- Add request/response DTOs in `Api/Dtos/Request/` and `Api/Dtos/Response/`
- Register any new Application services in `CompositionRoot.cs`
- Controllers must not contain business logic — delegate to Application services

### 5. Tests
- Add unit tests in `AiKnowledgeAssistant.Tests/`
- Mock all Application interfaces with Moq
- Name test methods as `MethodName_Scenario_ExpectedOutcome`

Do not add features, abstractions, or extensibility beyond what is explicitly described above.
