---
mode: ask
description: Review code against Clean Architecture and SOLID rules for this project
---

Review the following file (or the currently open file if none specified): `${input:filePath}`

Evaluate it against the architecture and conventions of this project:

### Clean Architecture Checks
- [ ] Does the class reference types from a layer it should not depend on? (e.g., Application referencing Infrastructure concrete types, Domain referencing Application)
- [ ] Are all external I/O operations behind interfaces defined in Application?
- [ ] Is any Azure SDK type (`AzureOpenAIClient`, `SearchClient`, `EmbeddingClient`) used outside of Infrastructure?
- [ ] Is any EF Core type (`DbContext`, `DbSet`) used outside of Infrastructure?

### SOLID Checks
- **S**: Does the class have more than one reason to change? If so, suggest a split.
- **O**: Would adding a new variant require modifying this class rather than adding a new implementation?
- **L**: If this implements an interface, does it fully honour the interface contract without throwing `NotImplementedException` or weakening the contract?
- **I**: Is this interface too broad? Should it be split (e.g., chat vs. embedding, read vs. write)?
- **D**: Does this class construct its own dependencies (`new SomeConcrete()`) instead of receiving them via constructor injection?

### Project-Specific Checks
- [ ] Are AI calls wrapped with Polly policies from `AiResiliencePolicies`?
- [ ] Is business logic leaking into controllers (should be in Application services)?
- [ ] Are exceptions caught too broadly, swallowing useful error context?
- [ ] Is `IFailureRecordReader` implemented by the correct class (not by `FailureRecord` model)?
- [ ] Are new DI registrations added to `CompositionRoot.cs` or `AddInfrastructure()` as appropriate?

### Output Format
For each issue found:
- **File and approximate line**
- **Rule violated**
- **Specific fix recommendation**

If no issues are found, say so explicitly and briefly explain why the code is well-structured.
