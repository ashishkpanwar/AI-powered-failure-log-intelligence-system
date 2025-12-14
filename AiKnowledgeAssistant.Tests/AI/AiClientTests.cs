using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;


namespace AiKnowledgeAssistant.Tests.AI
{
    public class AiClientTests
    {
        [Fact]
        public async Task Chat_ReturnsResponse()
        {
            using var host = TestHost.Build();
            var client = host.Services.GetRequiredService<IAiClient>();

            var response = await client.GetChatResponseAsync(
                "You are a helpful assistant.",
                "What is cosine similarity?",
                CancellationToken.None);

            Assert.False(string.IsNullOrWhiteSpace(response));
        }
    }

}
