using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Queries;

public sealed record FindSimilarFailuresQuery(
    string Content,
    string Environment,
    string ServiceName,
    int MinSeverity,
    bool OnlyActive = true,
    int Top = 5
);

