using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Shared.Configurations;

/// <summary>
/// ElasticSearch configurations
/// </summary>
public static class ElasticSearchConfigurations
{
    public static ElasticsearchSinkOptions ConfigureElastic(string environmentName)
    {
        var callingAssembly = Assembly.GetCallingAssembly().GetName().Name!;
        Console.WriteLine($" ---- - ------------- -------- --------- Caller: {callingAssembly}");

        return new ElasticsearchSinkOptions
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{callingAssembly.ToLower()}-{environmentName.ToLower()}-{DateTime.Now:dd:MM:yyyy}",
            NumberOfReplicas = 1,
            NumberOfShards = 2,
        };
    }
}