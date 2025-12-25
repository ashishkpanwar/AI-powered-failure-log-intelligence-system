using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Application.Mapping;
using AiKnowledgeAssistant.Ingestion.Application.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;

namespace AiKnowledgeAssistant.Ingestion.Infrastructure.LogSources;

public sealed class JsonFileLogSource : ILogSource
{
    private readonly string _directoryPath;
    private readonly IHostEnvironment _environment; // You need to implement or inject this

    public JsonFileLogSource(string directoryPath, IHostEnvironment environment)
    {
        _directoryPath = directoryPath;  //D:\AI-powered failure & log intelligence system\AiKnowledgeAssistant.Ingestion\mock-logs\
        _environment = environment;
    }

    public async Task<IReadOnlyList<RawLogEntry>> FetchAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        var results = new List<RawLogEntry>();
        var basePath = _environment.ContentRootPath;
        var logDirectory = Path.GetFullPath(
            Path.Combine(basePath, _directoryPath));

        if (!Directory.Exists(_directoryPath))
            return results;

        var files = Directory.GetFiles(_directoryPath, "*.json");

        foreach (var file in files)
        {
            await using var stream = File.OpenRead(file);

            var dtos = await JsonSerializer.DeserializeAsync<List<RawLogEntryDto>>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },cancellationToken);

            if (dtos is null)
                continue;

            foreach (var dto in dtos)
            {
                if (RawLogEntryMapper.TryMap(dto, out var entry))
                {
                    if (entry.Timestamp >= from && entry.Timestamp <= to)
                        results.Add(entry);
                }
                // else → bad log, ignored intentionally
            }
        }

        return results;
    }
}
