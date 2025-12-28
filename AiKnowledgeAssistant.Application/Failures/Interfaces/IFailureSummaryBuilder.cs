using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{
    public interface IFailureSummaryBuilder
    {
        FailureTechnicalSummary Build(IReadOnlyList<FailureRecord> records);
    }
}
