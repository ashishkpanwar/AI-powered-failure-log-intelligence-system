using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;

namespace AiKnowledgeAssistant.Application.Failures;

public sealed class FailureRetrievalService : IFailureRetrievalService
{
    private readonly IAiEmbeddingClient _embeddingClient;
    private readonly IFailureVectorStore _vectorStore;

    public FailureRetrievalService(
        IAiEmbeddingClient embeddingClient,
        IFailureVectorStore vectorStore)
    {
        _embeddingClient = embeddingClient;
        _vectorStore = vectorStore;
    }

    public async Task<IReadOnlyList<FailureRecord>> FindSimilarAsync(
        FindSimilarFailuresQuery query,
        CancellationToken cancellationToken)
    {
        var embedding = await _embeddingClient.GenerateEmbeddingAsync(
            query.Content,
            cancellationToken);

        var filter = BuildFilter(query);

        return await _vectorStore.SearchSimilarFailuresAsync(
            embedding,
            filter,
            query.Top,
            cancellationToken);
    }

    private static string BuildFilter(FindSimilarFailuresQuery query)
    {
        var filters = new List<string>
        {
            $"environment eq '{query.Environment}'",
            $"serviceName eq '{query.ServiceName}'",
            $"severity ge {query.MinSeverity}"
        };

        if (query.OnlyActive)
        {
            filters.Add("isActive eq true");
        }

        return string.Join(" and ", filters);

    }
}