namespace AiKnowledgeAssistant.Ingestion.Application.Normalization;

using AiKnowledgeAssistant.Ingestion.Application.Models;

public interface ILogNormalizer
{
    FailureRecord? Normalize(RawLogEntry log);
}
