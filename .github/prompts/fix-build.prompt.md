---
mode: agent
description: Diagnose and fix all compile errors in this .NET 10 solution
---

The solution `AiKnowledgeAssistant.sln` has build errors. Fix all of them.

### Approach

1. Read the compile error messages carefully — note the project, file, and line.
2. Before editing any file, read the full file and understand existing code.
3. Check the following common root causes in this project:
   - **Missing `using` / missing package reference**: The API project uses `ImplicitUsings` — check if a required namespace like `Microsoft.Extensions.DependencyInjection` needs an explicit `PackageReference` in the `.csproj`.
   - **`IServiceCollection` / `IConfiguration` not found in library projects**: Library projects (Application, Infrastructure) do not reference `Microsoft.Extensions.*` by default — add the appropriate `PackageReference`.
   - **`Predefined type 'System.Object' is not defined`**: This cascades from a missing core reference — fix the root cause, not each symptom separately.
   - **Wrong implementation registered for interface**: E.g., `services.AddScoped<IFailureRecordReader, FailureRecord>()` — `FailureRecord` is a model, not a repository. Find the correct implementing class.
   - **Namespace mismatch**: Some files (e.g., `IJobExecutionReader`) are defined in `AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces` but used in Application — verify `using` directives.
   - **Filename with trailing spaces**: Infrastructure has files like `AzureOpenAiClient .cs` — ensure file names and class names match.

4. After each file edit, verify the error is resolved before moving to the next.
5. Do not change logic, only fix compilation issues.

### Constraints
- Do not downgrade any package versions.
- Do not remove existing features or registrations.
- Preserve all `sealed`, `required`, and `init` modifiers already present.
