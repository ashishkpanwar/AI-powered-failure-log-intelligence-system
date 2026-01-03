using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Helpers
{
    public static class FailureExplanationPromptBuilder
    {
        private static string BuildPrompt(
        FailureTechnicalSummary jobSummary,
        WindowInsights insights)
            {
                var recurrenceText = insights.IsRecurringFailure
                    ? $"This job is part of a sequence of {insights.FailedJobCount} failed runs."
                    : "This appears to be the first failure after a successful run.";

                return $"""
                You are assisting with failure analysis.

                Current job failure details:
                - Error types: {string.Join(", ", jobSummary.ErrorTypes.Keys)}
                - Common messages: {string.Join(" | ", jobSummary.CommonErrorMessages)}
                - Job First Failed at : {jobSummary.FirstFailureAt}
                - Last failure occurred at: {jobSummary.LastFailureAt}
                

                Context:
                {recurrenceText}

                Instructions:
                - Explain what happened in simple terms
                - Do NOT guess the root cause
                - Do NOT suggest fixes
                - Be concise and cautious
                """;
            }

        }

    internal sealed class WindowInsights
    {
        public int FailedJobCount { get; init; }
        public bool IsRecurringFailure => FailedJobCount > 1;
        public IReadOnlyList<string> CommonErrorTypes { get; init; } = [];
    }



}
