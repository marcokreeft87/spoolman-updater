using LinqKit;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gateways;

internal class SpoolSpoolmanEndpoint : SpoolmanEndpoint<Spool>, ISpoolEndpoint
{
    private readonly IVendorEndpoint vendorEndpoint;
    private readonly IFilamentEndpoint filamentEndpoint;

    public SpoolSpoolmanEndpoint(
        SpoolmanConfiguration configuration,
        IVendorEndpoint vendorEndpoint,
        IFilamentEndpoint filamentEndpoint) : base(configuration)
    {
        this.vendorEndpoint = vendorEndpoint;
        this.filamentEndpoint = filamentEndpoint;
    }

    protected override string Endpoint => "spool";

    public async Task<List<Spool>> GetAllAsync() => await GetAllAsync(string.Empty, false);

    public async Task<List<Spool>> GetCurrentSpoolsInTray(string trayId)
    {
        var jsonEncoded = JsonSerializer.Serialize(trayId, JsonOptions);

        var allSpools = await GetAllAsync(string.Empty, false);

        return allSpools?.Where(spool => spool.Extra.ContainsKey(FilamentQueryConstants.ActiveTrayId) && spool.Extra[FilamentQueryConstants.ActiveTrayId] == jsonEncoded).ToList() ?? new List<Spool>();
    }

    public async Task<Spool> GetOrCreateSpool(string vendorName, string material, string color, string activeTrayId, string tagUid)
    {
        var predicate = PredicateBuilder.New<Spool>(true);

        if (!string.IsNullOrEmpty(activeTrayId))
        {
            var jsonEncoded = JsonSerializer.Serialize(activeTrayId, JsonOptions);

            predicate = predicate.And(spool => spool.Extra.ContainsKey(FilamentQueryConstants.ActiveTrayId) && spool.Extra[FilamentQueryConstants.ActiveTrayId] == jsonEncoded);
        }        
        else if (!string.IsNullOrEmpty(tagUid))
        {
            var jsonEncoded = JsonSerializer.Serialize(tagUid, JsonOptions);
            predicate = predicate.And(spool => spool.Extra.ContainsKey(FilamentQueryConstants.TagUid) && spool.Extra[FilamentQueryConstants.TagUid] == jsonEncoded);
        }
        else 
        {
            if (!string.IsNullOrEmpty(material))
            {
                predicate = predicate.And(spool => spool.Filament.Material == material);
            }

            if (!string.IsNullOrEmpty(color))
            {
                predicate = predicate.And(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);
            }
        }        

        var allBrandSpools = await GetAllAsync(string.Empty, false);

        Spool? matchingSpool = allBrandSpools?.FirstOrDefault(predicate);

        matchingSpool ??= await CreateSpoolAsync(vendorName, color.Substring(1, 6), material, activeTrayId, tagUid);

        return matchingSpool;
    }

    public async Task<bool> SetActiveTray(int spoolId, string activeTrayId)
    {
        var spool = await GetByIdAsync(spoolId.ToString());
        if (spool == null)
            throw new InvalidOperationException($"Spool with ID {spoolId} not found.");

        spool.Extra[FilamentQueryConstants.ActiveTrayId] = JsonSerializer.Serialize(activeTrayId, JsonOptions);

        return await UpdateAsync(spool.Id.Value, new
        {
            extra = new
            {
                active_tray = JsonSerializer.Serialize(activeTrayId, JsonOptions)
            }
        });
    }

    public async Task<bool> UseSpoolWeight(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await HttpClient.PutAsJsonAsync($"{Endpoint}/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    private async Task<Spool?> CreateSpoolAsync(string vendorName, string color, string material, string activeTrayId, string tagUid)
    {
        var vendor = await vendorEndpoint.GetOrCreate(vendorName);

        var filament = await filamentEndpoint.GetOrCreate(vendor, color, material);

        var extra = new Dictionary<string, string>();

        if (!Spool.IsEmptyTag(tagUid))
        {
            extra[FilamentQueryConstants.TagUid] = JsonSerializer.Serialize(tagUid, JsonOptions);
        }

        if (!string.IsNullOrEmpty(activeTrayId))
        {
            extra[FilamentQueryConstants.ActiveTrayId] = JsonSerializer.Serialize(activeTrayId, JsonOptions);
        }

        var newSpool = new Spool
        {
            FilamentId = filament.Id,
            InitialWeight = 1000,  // Default values, adjust as needed
            RemainingWeight = 1000,
            SpoolWeight = 250,
            Extra = extra
        };

        return await PostAsync(newSpool);
    }

    public async Task<Spool> GetByIdAsync(int spoolId) => await GetByIdAsync(spoolId.ToString());
}
