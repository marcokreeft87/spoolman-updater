using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        if (configuration.AMSEntities != null && configuration.AMSEntities.Any())
        {
            foreach (var amsEntity in configuration.AMSEntities)
            {
                var trays = await GetAllTrayInfoAsync(amsEntity);

                var amsEntityInfo = new AMSEntity
                {
                    Id = amsEntity,
                    Trays = trays
                };

                amsEntities.Add(amsEntityInfo);
            }
        }
        else if (configuration.AMSEntities != null && configuration.TrayEntities.Any())
        {
            var groupedByAms = configuration.TrayEntities
                .GroupBy(entity =>
                {
                    var match = Regex.Match(entity, @"ams_(\d+)");
                    return match.Success ? $"AMS {match.Groups[1].Value}" : "Unknown";
                });

            foreach (var group in groupedByAms)
            {
                var trayTasks = group.Select(entity => GetTrayInfoAsync(entity)).ToList();
                var trays = await Task.WhenAll(trayTasks);

                var amsEntityInfo = new AMSEntity
                {
                    Id = group.Key,
                    Trays = trays.Where(tray => tray != null).ToList()
                };

                amsEntities.Add(amsEntityInfo);
            }
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
        var allStates = await _httpClient.GetFromJsonAsync<List<HaEntity>>($"{_baseUrl}/api/states");

        var regex = new Regex($@"^sensor\.{amsEntity}_.*_\d+$", RegexOptions.IgnoreCase);

        var sensors = allStates
            .Where(e => regex.IsMatch(e.EntityId))
            .ToList();

        var trayTasks = new List<Task<TrayInfo?>>();

        sensors.ForEach(sensor => trayTasks.Add(GetTrayInfoAsync(sensor.EntityId)));

        var results = await Task.WhenAll(trayTasks);

        return new List<TrayInfo?>(results);
    }
}
