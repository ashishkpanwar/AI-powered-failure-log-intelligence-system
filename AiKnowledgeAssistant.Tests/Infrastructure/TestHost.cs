using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AiKnowledgeAssistant.Api;

namespace AiKnowledgeAssistant.Tests.Infrastructure
{
    public static class TestHost
    {
        public static IHost Build()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    // 🔹 reuse Program.cs registrations
                        CompositionRoot.ConfigureServices(
                        services,
                        context.Configuration);
                })
                .Build();
        }
    }

}
