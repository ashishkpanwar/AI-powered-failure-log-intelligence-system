using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Application.Normalization;
using AiKnowledgeAssistant.Ingestion.Application.Pipeline;
using AiKnowledgeAssistant.Ingestion.Infrastructure.LogSources;
using AiKnowledgeAssistant.Ingestion.Infrastructure.Search;
using Azure;
using Azure.AI.OpenAI;
using Azure.Search.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Embeddings;

var host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices((config, services) =>
    {
        var configuration = config.Configuration;
        // 🔹 Log source (mock for now)
        services.AddSingleton<ILogSource>(sp =>
            {
                var env = sp.GetRequiredService<IHostEnvironment>();
                return new JsonFileLogSource(configuration["Ingestion:LogDirectory"]!, env);
            });

        // 🔹 Normalizer
        services.AddSingleton<ILogNormalizer, DefaultLogNormalizer>();

        // 🔹 Azure AI Search client
        services.AddSingleton(_ =>
        {
            var endpoint = new Uri(configuration["AzureSearchService:Endpoint"]!);
            var apiKey = new AzureKeyCredential(configuration["AzureSearchService:ApiKey"]!);
            return new SearchClient(endpoint, configuration["AzureSearchService:IndexName"]!, apiKey);
        });

        services.AddSingleton(sp =>
        {
            var endpoint = new Uri(configuration["AzureOpenAI:Endpoint"]!);
            var key = configuration["AzureOpenAI:ApiKey"]!;
            return new AzureOpenAIClient(endpoint, new AzureKeyCredential(key));
        });

        // 🔹 Embedding generator (reuse existing logic / client)
        services.AddSingleton<Func<string, CancellationToken, Task<float[]>>>(
            sp => async (text, ct) =>
            {
                try
                {
                    var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
                    var deployment = configuration["AzureOpenAI:EmbeddingDeployment"]!;
                    var _embeddingClient = azureClient.GetEmbeddingClient(deployment);
                    var response = await _embeddingClient.GenerateEmbeddingAsync(text,null,ct);
                     return response.Value.ToFloats().ToArray();
                
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException("Wire embedding client");
                }
            });

        services.AddSingleton<FailureIngestionService>();
        services.AddSingleton<FailureIngestionPipeline>();
    })
    .Build();

    using (host)
    {
        var pipeline =
            host.Services.GetRequiredService<FailureIngestionPipeline>();

        await pipeline.RunAsync(
            from: DateTimeOffset.UtcNow.AddMonths(-4),
            to: DateTimeOffset.UtcNow,
            cancellationToken: CancellationToken.None);
    }
