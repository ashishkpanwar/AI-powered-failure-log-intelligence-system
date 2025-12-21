namespace AiKnowledgeAssistant.Ingestion.Application.Models;

public sealed class RawLogEntry
{
    public required DateTimeOffset Timestamp { get; init; }
    public required string Level { get; init; }
    public required string Message { get; init; }

    public string? ServiceName { get; init; }
    public string? WorkflowName { get; init; }
    public string? Source { get; init; }

    public IDictionary<string, object> Properties { get; init; }
        = new Dictionary<string, object>();
}
