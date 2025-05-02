using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gateways;

internal abstract class SpoolmanEndpoint<TSpoolmanEntity> : ISpoolmanEndpoint<TSpoolmanEntity>
    where TSpoolmanEntity : class
{
    private readonly ILogger<SpoolmanEndpoint<TSpoolmanEntity>> logger;

    protected readonly HttpClient HttpClient;
    protected readonly JsonSerializerOptions JsonOptions;

    protected abstract string Endpoint { get; }

    public SpoolmanEndpoint(SpoolmanConfiguration configuration, ILogger<SpoolmanEndpoint<TSpoolmanEntity>> logger)
    {
        this.logger = logger;

        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri($"{configuration.Url}/api/v1/");

        // Configure snake_case naming policy
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<List<TSpoolmanEntity>?> GetAllAsync(string query = "", bool useQueryParams = true)
    {
        logger.LogInformation($"Getting all {typeof(TSpoolmanEntity).Name} with query: {query}");

        var response = await HttpClient.GetAsync($"{Endpoint}{(useQueryParams ? "?" : string.Empty)}{query}");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError($"Failed to get {typeof(TSpoolmanEntity).Name}. Status code: {response.StatusCode}");
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<TSpoolmanEntity>>(JsonOptions);
    }

    public async Task<TSpoolmanEntity?> GetByIdAsync(string id)
    {
        logger.LogInformation($"Getting {typeof(TSpoolmanEntity).Name} with ID: {id}");

        var response = await HttpClient.GetAsync($"{Endpoint}/{id}");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError($"Failed to get {typeof(TSpoolmanEntity).Name} with ID: {id}. Status code: {response.StatusCode}");
            return null;
        }

        return await response.Content.ReadFromJsonAsync<TSpoolmanEntity>(JsonOptions);
    }

    public async Task<TSpoolmanEntity?> PostAsync(TSpoolmanEntity newEntity)
    {
        logger.LogInformation($"Creating new {typeof(TSpoolmanEntity).Name}");

        var createVendorResponse = await HttpClient.PostAsJsonAsync(Endpoint, newEntity, JsonOptions);

        if (!createVendorResponse.IsSuccessStatusCode)
        {
            logger.LogError($"Failed to create {typeof(TSpoolmanEntity).Name}. Status code: {createVendorResponse.StatusCode}");
            return null;
        }

        return await createVendorResponse.Content.ReadFromJsonAsync<TSpoolmanEntity>();
    }

    public async Task<bool> UpdateAsync(int id, object patch)
    {
        logger.LogInformation($"Updating {typeof(TSpoolmanEntity).Name} with ID: {id}");

        var updateVendorResponse = await HttpClient.PatchAsJsonAsync($"{Endpoint}/{id}", patch, JsonOptions);

        if (!updateVendorResponse.IsSuccessStatusCode)
        {
            logger.LogError($"Failed to update {typeof(TSpoolmanEntity).Name} with ID: {id}. Status code: {updateVendorResponse.StatusCode}");
            return false;
        }

        return updateVendorResponse.IsSuccessStatusCode;
    }
}