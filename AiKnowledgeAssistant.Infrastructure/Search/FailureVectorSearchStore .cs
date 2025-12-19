using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace AiKnowledgeAssistant.Infrastructure.Search;

public sealed class FailureVectorSearchStore : IFailureVectorStore
{
    private readonly SearchClient _searchClient;

    private static readonly string[] SelectFields =
    [
        "id",
        "content",
        "timestamp",
        "serviceName",
        "workflowName",
        "environment",
        "errorType",
        "severity",
        "isActive",
        "source"
    ];

    public FailureVectorSearchStore(SearchClient searchClient)
    {
        _searchClient = searchClient;
    }

    public async Task<IReadOnlyList<FailureRecord>> SearchSimilarFailuresAsync(
        float[] embedding,
        string filter,
        int top,
        CancellationToken ct)
    {
        var options = new SearchOptions
        {
            Size = top,
            Filter = filter,
            VectorSearch = new()
            {
                Queries =
                {
                    new VectorizedQuery(embedding)
                    {
                        Fields = { "contentVector" }
                    }
                }
            }
        };

        foreach (var field in SelectFields)
        {
            options.Select.Add(field);
        }

        var results = await _searchClient.SearchAsync<FailureRecord>(
            searchText: null,
            options,
            ct);

        var failures = new List<FailureRecord>();

        await foreach (var result in results.Value.GetResultsAsync())
        {
            failures.Add(result.Document);
        }

        return failures;
    }
}
