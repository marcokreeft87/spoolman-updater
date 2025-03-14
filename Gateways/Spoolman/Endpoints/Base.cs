﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gateways;

internal class SpoolmanEndoint<TSpoolmanEntity> : ISpoolmanEndpoint<TSpoolmanEntity>
    where TSpoolmanEntity : class
{
    protected readonly HttpClient HttpClient;
    protected readonly JsonSerializerOptions JsonOptions;

    public SpoolmanEndoint(SpoolmanConfiguration configuration)
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri($"{configuration.Url}/api/v1/");

        // Configure snake_case naming policy
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}