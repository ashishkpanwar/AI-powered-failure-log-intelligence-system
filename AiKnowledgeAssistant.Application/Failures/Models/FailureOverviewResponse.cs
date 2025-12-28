using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureOverviewResponse
    {
        public string JobId { get; init; } = default!;
        public string WorkflowId { get; init; } = default!;
        public string Environment { get; init; } = default!;

        public FailureTechnicalSummary TechnicalSummary { get; init; } = default!;
        public string AiSummary { get; init; } = default!;
    }

}
