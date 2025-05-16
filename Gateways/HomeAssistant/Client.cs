using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Gateways;

public class HomeAssistantClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _token;
    private readonly HomeAssistantConfiguration configuration;
    private readonly JsonSerializerOptions JsonOptions;

    public HomeAssistantClient(HomeAssistantConfiguration configuration)
    {
        _baseUrl = configuration.Url;
        _token = configuration.Token;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        this.configuration = configuration;

        JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<List<AMSEntity>> GetAmsInfoAsync()
    {
        var amsEntities = new List<AMSEntity>();
        foreach(var amsEntity in configuration.AMSEntities)
        {
            var trays = await GetAllTrayInfoAsync(amsEntity);

            var amsEntityInfo = new AMSEntity
            {
                Id = amsEntity,
                Trays = trays
            };

            amsEntities.Add(amsEntityInfo);
        }  
        
        return amsEntities;
    }

    public async Task<bool> SetPrintTraySpool(string trayEntityId, string filamentColor, string filamentType, string filamentFinish, int nozzleTempMin, int nozzleTempMax)
    {
        filamentType = filamentType.Replace("+", string.Empty);

        var requestEntity = new BambuServiceRequest()
        {
            EntityId = [$"sensor.{trayEntityId}"],
            TrayInfoIdx = GetTrayInfoIdx(filamentType, filamentFinish.Trim()) ?? "",
            TrayColor = $"{filamentColor}FF",
            TrayType = filamentType,
            NozzleTempMin = nozzleTempMin > 0 ? nozzleTempMin : 220,
            NozzleTempMax = nozzleTempMax > 0 ? nozzleTempMax : 250
        };

        var response = await _httpClient.PostAsJsonAsync<BambuServiceRequest>($"{_baseUrl}/api/services/bambu_lab/set_filament", requestEntity, JsonOptions);

        return response.IsSuccessStatusCode;
    }

    public async Task<TrayInfo?> GetExternalSpoolAsync() => await GetTrayInfoAsync(configuration.ExternalSpoolEntity);

    private string GetTrayInfoIdx(string filamentType, string filamentFinish)
    {
        Dictionary<string, string> MaterialMap = new()
        {
            // PLA Variants
            { "PLA Basic", "GFA00" },
            { "PLA Matte", "GFA01" },
            { "PLA Metal", "GFA02" },
            { "PLA Impact", "GFA03" },
            { "PLA Silk", "GFA05" },
            { "PLA Marble", "GFA07" },
            { "PLA Sparkle", "GFA08" },
            { "PLA Tough", "GFA09" },
            { "PLA Aero", "GFA11" },
            { "PLA Glow", "GFA12" },
            { "PLA Dynamic", "GFA13" },
            { "PLA Galaxy", "GFA15" },
            { "PLA-CF", "GFA50" },

            // ABS and ASA Variants
            { "ABS", "GFB00" },
            { "ASA", "GFB01" },
            { "ASA-Aero", "GFB02" },
            { "ABS-GF", "GFB50" },
            { "ASA-CF", "GFB51" },

            // Supports
            { "Support W", "GFS00" },
            { "Support G", "GFS01" },
            { "Support For PLA", "GFS02" },
            { "Support For PA/PET", "GFS03" },
            { "PVA", "GFS04" },
            { "Support For PLA/PETG", "GFS05" },
            { "Support for ABS", "GFS06" },

            // Other Materials
            { "PET-CF", "GFT01" },
            { "PPS-CF", "GFT02" },
            { "TPU 95A HF", "GFU00" },
            { "TPU 95A", "GFU01" },
            { "TPU for AMS", "GFU02" }
        };

        return MaterialMap.TryGetValue($"{filamentType} {filamentFinish}", out var materialName)
            ? materialName
            : null;
    }

    private async Task<TrayInfo?> GetTrayInfoAsync(string entity)
    {
        var response = await _httpClient.GetFromJsonAsync<HomeAssistantState>($"{_baseUrl}/api/states/{entity}");

        var trayInfo = response?.Attributes;

        trayInfo.Id = entity.Replace("sensor.", string.Empty);

        return trayInfo;
    }

    private async Task<List<TrayInfo?>> GetAllTrayInfoAsync(string amsEntity)
    {
        var trayTasks = new List<Task<TrayInfo?>>
        {
            GetTrayInfoAsync($"sensor.{amsEntity.ToLower()}_tray_1"),
            GetTrayInfoAsync($"sensor.{amsEntity.ToLower()}_tray_2"),
            GetTrayInfoAsync($"sensor.{amsEntity.ToLower()}_tray_3"),
            GetTrayInfoAsync($"sensor.{amsEntity.ToLower()}_tray_4")
        };

        var results = await Task.WhenAll(trayTasks);

        return new List<TrayInfo?>(results);
    }
}
