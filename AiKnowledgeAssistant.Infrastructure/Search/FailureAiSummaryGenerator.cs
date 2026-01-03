using AiKnowledgeAssistant.Application.AI.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Search
{
    public sealed class FailureAiSummaryGenerator
    : IFailureAiSummaryGenerator
    {
        private readonly IAiClient _chatClient;

        public FailureAiSummaryGenerator(IAiClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<string> GenerateAsync(
            FailureTechnicalSummary jobSummary,
            IReadOnlyList<FailureRecord> windowFailureRecords)
        {
            var insights = AnalyzeWindow(windowFailureRecords);
            var systemPrompt = BuildSystemPrompt();
            var userPrompt = BuildUserPrompt(jobSummary, insights);

            return await _chatClient.GetChatResponseAsync(systemPrompt, userPrompt, default);
        }
        private static string BuildSystemPrompt()
        {
            return """
                You are an assistant helping explain job failures.

                Rules:
                - Explain what happened in simple terms
                - Be factual and cautious
                - Do NOT guess the root cause
                - Do NOT suggest fixes
                - If this is the first failure, say so
                - If failures are recurring, mention recurrence without speculation
                """;
        }

        private static string BuildUserPrompt(
        FailureTechnicalSummary jobSummary,
        WindowInsights insights)
            {
            var recurrenceText = insights.IsRecurringFailure
                ? $"""
                This job is part of a sequence of {insights.FailedJobCount} failed runs. The top 2 common errors are
                {string.Join(" and ", insights.CommonErrorTypes)}. 
                """
                : "This appears to be the first failure after a successful run.";

            return $"""
                Current job failure details:
                - Error types: {string.Join(", ", jobSummary.ErrorTypes.Keys)}
                - Common messages: {string.Join(" | ", jobSummary.CommonErrorMessages)}
                - Job First Failed at : {jobSummary.FirstFailureAt}
                - Last failure occurred at: {jobSummary.LastFailureAt}

                Context:
                {recurrenceText}

                Please explain this failure for a non-technical audience.
                """;
        }

        private static WindowInsights AnalyzeWindow(
    IReadOnlyList<FailureRecord> records)
        {
            return new WindowInsights
            {
                FailedJobCount = records
                    .Select(r => r.JobId)
                    .Distinct()
                    .Count(),

                CommonErrorTypes = records
                    .GroupBy(r => r.ErrorType)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(2)
                    .ToList()
            };
        }

    }

    internal sealed class WindowInsights
        {
            public int FailedJobCount { get; init; }
            public bool IsRecurringFailure => FailedJobCount > 1;
            public IReadOnlyList<string> CommonErrorTypes { get; init; } = [];

    }

    
}
