---
mode: agent
description: Write xUnit tests for a class in this project using Moq
---

Write xUnit tests for `${input:className}` in `${input:projectOrNamespace}`.

Follow these conventions from the project:

### Test Structure
- Use the `[Fact]` attribute for single-case tests and `[Theory]` + `[InlineData]` for parameterised cases.
- Name each test method: `MethodName_Scenario_ExpectedOutcome`
- One `Assert` concept per test (multiple asserts are acceptable only if they verify the same outcome).

### Mocking
- Use `Moq` (`Mock<T>`, `.Setup(...)`, `.Verify(...)`) for all Application interfaces (`IAiClient`, `IAiEmbeddingClient`, `IFailureVectorStore`, `IJobExecutionReader`, `IFailureWindowResolver`, `IFailureRecordReader`, etc.)
- Never mock Domain models — construct them directly.
- Never mock `sealed` Infrastructure classes — test them via their interface.

### Test Scenarios to Cover
For **services/use-cases** (`FailureOverviewService`, `FailureWindowResolver`, `FailureRetrievalService`):
- Happy path returns expected result
- Dependency returns empty/null — verify correct exception or fallback
- Dependency throws — verify propagation or mapping

For **builders/normalizers** (`FailureSummaryBuilder`, `DefaultLogNormalizer`, `DefaultTokenGuardrail`):
- Valid input produces expected output
- Boundary values (0 records, 1, 3, 4 for `FailureConfidence` thresholds)
- Invalid/empty input throws `InvalidOperationException` or returns `null`

For **controllers** (if testing at HTTP level):
- Use `TestHostFixture` from `AiKnowledgeAssistant.Tests/` to spin up the full DI graph
- Assert HTTP status codes, not just service return values

### What NOT to Do
- Do not test Azure SDK classes directly.
- Do not make real HTTP or database calls in unit tests.
- Do not add integration tests that depend on Azure without a clear fixture setup.
