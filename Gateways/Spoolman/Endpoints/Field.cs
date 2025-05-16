using System.Net.Http.Json;

namespace Gateways;

internal class FieldSpoolmanEndpoint(SpoolmanConfiguration configuration) : SpoolmanEndpoint<Field>(configuration), IFieldEndpoint
{
    private EntityType FieldType { get; set; } = EntityType.Spool;

    protected override string Endpoint => "field";

    public async Task<bool> CheckFieldExistence()
    {
        var tasks = new[]
        {
            GetFieldAsync(FieldType, "tag"),
            GetFieldAsync(FieldType, "active_tray"),
            GetFieldAsync(FieldType, "barcode"),
        };

        var results = await Task.WhenAll(tasks);

        // Return true if any of the results are not null
        return results.Any(field => field != null);
    }

    private async Task<Field> GetFieldAsync(EntityType fieldType, string key)
    {
        Field? field = null;
        var fields = await GetFieldsAsync(fieldType);

        field = fields?.FirstOrDefault(f => f.Key == key);

        if (fields != null && fields.Any())
        {
            field = fields.FirstOrDefault(field => field.Key.ToLower() == key.ToLower());
        }

        if (field == null)
        {
            var newFields = await CreateField(fieldType, key);

            field = newFields.FirstOrDefault(field => field.Key.ToLower() == key.ToLower());
        }

        return field;
    }

    private async Task<List<Field>> CreateField(EntityType fieldType, string key)
    {
        var field = new Field
        {
            Key = key,
            EntityType = fieldType.ToString(),
            Name = key,
            FieldType = "text"
        };

        return await PostAsync($"/{fieldType.ToString().ToLower()}/{key}", field) ?? new List<Field>();
    }

    private async Task<List<Field>?> GetFieldsAsync(EntityType fieldType) =>
        await GetAllAsync($"/{fieldType.ToString().ToLower()}", false);

    private async Task<List<Field>?> PostAsync(string extraEndpointPath, Field newEntity)
    {
        var createVendorResponse = await HttpClient.PostAsJsonAsync($"{Endpoint}{extraEndpointPath}", newEntity, JsonOptions);

        return createVendorResponse.IsSuccessStatusCode ? await createVendorResponse.Content.ReadFromJsonAsync<List<Field>>() : null;
    }
}