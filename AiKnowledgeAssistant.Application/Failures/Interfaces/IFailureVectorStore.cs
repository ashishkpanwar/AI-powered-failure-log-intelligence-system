using AiKnowledgeAssistant.Application.Failures.Models;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces;

public interface IFailureVectorStore
{
    Task<IReadOnlyList<FailureRecord>> SearchSimilarFailuresAsync(
        float[] embedding,
        string filter,
        int top,
        CancellationToken cancellationToken);
}
