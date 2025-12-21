using System.Text.Json;
using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Application.Models;

namespace AiKnowledgeAssistant.Ingestion.Infrastructure.LogSources;

public sealed class JsonFileLogSource : ILogSource
{
    private readonly string _directoryPath;

    public JsonFileLogSource(string directoryPath)
    {
        _directoryPath = directoryPath;  //D:\AI-powered failure & log intelligence system\AiKnowledgeAssistant.Ingestion\mock-logs\
    }

    public async Task<IReadOnlyList<RawLogEntry>> FetchAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        var results = new List<RawLogEntry>();

        if (!Directory.Exists(_directoryPath))
            return results;

        var files = Directory.GetFiles(_directoryPath, "*.json");

        foreach (var file in files)
        {
            await using var stream = File.OpenRead(file);

            var logs = await JsonSerializer.DeserializeAsync<List<RawLogEntry>>(
                stream,
                cancellationToken: cancellationToken);

            if (logs is null)
                continue;

            results.AddRange(logs.Where(log=> log.Timestamp >= from && log.Timestamp <= to));

            //foreach (var log in logs)
            //{
            //    if (log.Timestamp >= from && log.Timestamp <= to)
            //    {
            //        results.Add(log);
            //    }
            //}
        }

        return results;
    }
}
