namespace AiKnowledgeAssistant.Ingestion.Application.Models;

public sealed class FailureRecord
{
    public required string Id { get; init; }
    public required string Content { get; init; }
    public required DateTimeOffset Timestamp { get; init; }

    public required string ServiceName { get; init; }
    public required string WorkflowName { get; init; }
    public required string Environment { get; init; }
    public required string ErrorType { get; init; }

    public required int Severity { get; init; }
    public required bool IsActive { get; init; }
    public required string Source { get; init; }
}
