using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models;

public sealed class FailureRecord
{
    public string Id { get; init; } = default!;
    public string Content { get; init; } = default!;
    public DateTimeOffset Timestamp { get; init; }
    public string ServiceName { get; init; } = default!;
    public string WorkflowName { get; init; } = default!;
    public string Environment { get; init; } = default!;
    public string ErrorType { get; init; } = default!;
    public int Severity { get; init; }
    public bool IsActive { get; init; }
    public string Source { get; init; } = default!;
}

