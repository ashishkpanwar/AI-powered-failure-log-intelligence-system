---
mode: ask
description: Explain what a job failure means and trace it through the codebase
---

A job with ID `${input:jobId}` has failed. Trace the full execution path for this failure through the codebase and explain:

1. How `FailureWindowResolver` resolves the continuous failure window starting from this job ID — reference `IJobExecutionReader.GetFailureWindowAsync` and the lookback logic.
2. How `FailureOverviewService.GetOverviewAsync` orchestrates the window, record loading, summary building, and AI generation steps.
3. What `FailureSummaryBuilder` computes (`FailureConfidence`, error type grouping, first/last timestamps).
4. What prompt `FailureAiSummaryGenerator` sends to Azure OpenAI — show both the system and user prompt templates.
5. Any error path that could throw `InvalidOperationException` and how `FailureOverviewController` maps it to HTTP status codes.

Use the actual class names, method signatures, and namespace paths from this codebase. Do not speculate about code that does not exist.
