using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Queries;

public sealed record FindSimilarFailuresQuery(
    string Environment,
    string JobId,
    int MinSeverity,
    string? Question,
    bool OnlyActive = true,
    int Top = 5
);

