namespace AiKnowledgeAssistant.Ingestion.Application.Models;

public sealed class RawLogEntryDto
{
    public DateTimeOffset? Timestamp { get; init; }
    public string? Level { get; init; }
    public string? Message { get; init; }

    public string? ServiceName { get; init; }
    public string? WorkflowName { get; init; }
    public string? Source { get; init; }

    public IDictionary<string, object>? Properties { get; init; }
}
