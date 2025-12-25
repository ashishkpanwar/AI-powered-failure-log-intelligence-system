using AiKnowledgeAssistant.Ingestion.Application.Models;

namespace AiKnowledgeAssistant.Ingestion.Application.Mapping;

public static class RawLogEntryMapper
{
    public static bool TryMap(
        RawLogEntryDto dto,
        out RawLogEntry entry)
    {
        entry = default!;

        if (dto.Timestamp is null ||
            string.IsNullOrWhiteSpace(dto.Level) ||
            string.IsNullOrWhiteSpace(dto.Message))
        {
            return false;
        }

        entry = new RawLogEntry
        {
            Timestamp = dto.Timestamp.Value,
            Level = dto.Level,
            Message = dto.Message,
            ServiceName = dto.ServiceName,
            WorkflowName = dto.WorkflowName,
            Source = dto.Source,
            Properties = dto.Properties ?? new Dictionary<string, object>()
        };

        return true;
    }
}
