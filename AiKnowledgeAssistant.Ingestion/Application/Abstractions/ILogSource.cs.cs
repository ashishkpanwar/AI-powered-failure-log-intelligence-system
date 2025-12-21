namespace AiKnowledgeAssistant.Ingestion.Application.Abstractions;

using AiKnowledgeAssistant.Ingestion.Application.Models;

public interface ILogSource
{
    Task<IReadOnlyList<RawLogEntry>> FetchAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken);
}
