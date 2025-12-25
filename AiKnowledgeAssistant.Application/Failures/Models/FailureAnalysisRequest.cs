using AiKnowledgeAssistant.Application.Failures.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureAnalysisRequest
    {
        public required string Environment { get; init; }
        public required string JobId { get; init; }

        public int MinSeverity { get; init; } = 3;
        public bool OnlyActive { get; init; } = true;

        public string? Question { get; init; }
    }
}
