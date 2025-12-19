using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces;

public interface IFailureRetrievalService
{
    Task<IReadOnlyList<FailureRecord>> FindSimilarAsync(
        FindSimilarFailuresQuery query,
        CancellationToken cancellationToken);
}
