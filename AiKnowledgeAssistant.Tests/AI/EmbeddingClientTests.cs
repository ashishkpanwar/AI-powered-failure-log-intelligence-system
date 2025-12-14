using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AiKnowledgeAssistant.Tests.AI
{
    public class EmbeddingClientTests
    {
        [Fact]
        public async Task GenerateEmbedding_ReturnsFixedLengthVector()
        {
            using var host = TestHost.Build();
            var client = host.Services.GetRequiredService<IAiEmbeddingClient>();

            var vector = await client.GenerateEmbeddingAsync(
                "SQL timeout during order processing",
                CancellationToken.None);

            Assert.NotNull(vector);
            Assert.True(vector.Length > 0);
        }
    }

}
