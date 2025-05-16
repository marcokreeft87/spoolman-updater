using System.Net.Http.Json;

namespace Gateways;

public class HomeAssistantClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _token;
    private readonly HomeAssistantConfiguration configuration;

    public HomeAssistantClient(HomeAssistantConfiguration configuration)
    {
        _baseUrl = configuration.Url;
        _token = configuration.Token;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        this.configuration = configuration;
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

    public async Task<TrayInfo?> GetExternalSpoolAsync() => await GetTrayInfoAsync(configuration.ExternalSpoolEntity);

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
