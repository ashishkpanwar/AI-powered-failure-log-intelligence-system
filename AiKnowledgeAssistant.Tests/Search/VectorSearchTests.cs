using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Search;
using AiKnowledgeAssistant.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AiKnowledgeAssistant.Tests.Search
{

    public class VectorSearchTests
    {
        [Fact]
        public async Task Search_ReturnsRelevantChunks()
        {
            using var host = TestHost.Build();

            var embeddingClient = host.Services.GetRequiredService<IAiEmbeddingClient>();
            var searchStore = host.Services.GetRequiredService<IVectorSearchStore>();

            var queryVector = await embeddingClient.GenerateEmbeddingAsync(
                "Database timeout error",
                CancellationToken.None);

            var results = await searchStore.SearchAsync(
                queryVector,
                topK: 3,
                CancellationToken.None);

            Assert.NotEmpty(results);
        }
    }

}
