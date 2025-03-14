﻿using System.Net.Http.Json;

namespace Gateways;

internal class SpoolSpoolmanEndoint : SpoolmanEndpoint<Spool>, ISpoolEndpoint
{
    private readonly IVendorEndpoint vendorEndpoint;
    private readonly IFilamentEndpoint filamentEndpoint;

    public SpoolSpoolmanEndoint(
        SpoolmanConfiguration configuration,
        IVendorEndpoint vendorEndpoint,
        IFilamentEndpoint filamentEndpoint) : base(configuration)
    {
        this.vendorEndpoint = vendorEndpoint;
        this.filamentEndpoint = filamentEndpoint;
    }

    protected override string Endpoint => "spool";

    public async Task<Spool> GetOrCreateSpool(string brand, string material, string color, string tagUid)
    {
        // TODO Mapping
        if (Spool.IsEmptyTag(tagUid) && brand == "Bambu")
            brand = "Sunlu";

        // Fetch all spools from Spoolman
        var query = $"filament.vendor.name={brand}";

        if (!string.IsNullOrEmpty(material))
        {
            query += $"&filament.material={material}";
        }

        var allBrandSpools = await GetAllAsync(query);

        Spool? matchingSpool = null;
        if (allBrandSpools != null && allBrandSpools.Any())
        {
            var colorMatchingSpools = allBrandSpools.Where(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);

            matchingSpool = !Spool.IsEmptyTag(tagUid) 
                ? colorMatchingSpools.FirstOrDefault(spool => spool.Extra["tag"] == tagUid)
                : colorMatchingSpools.FirstOrDefault();
        }

        matchingSpool ??= await CreateSpoolAsync(brand, color.Substring(1, 6), material, tagUid);

        return matchingSpool;
    }

    public async Task<bool> UseSpoolWeight(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await HttpClient.PutAsJsonAsync($"{Endpoint}/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    private async Task<Spool?> CreateSpoolAsync(string vendorName, string color, string material, string tagUid)
    {
        var vendor = await vendorEndpoint.GetOrCreate(vendorName);

        var filament = await filamentEndpoint.GetOrCreateFilament(vendor, color, material);

        var newSpool = new Spool
        {
            FilamentId = filament.Id,
            InitialWeight = 1000,  // Default values, adjust as needed
            RemainingWeight = 1000,
            SpoolWeight = 250,
            Extra = !Spool.IsEmptyTag(tagUid) ? new Dictionary<string, string>
            {
                { "tag", "0000000000000000" }
            } : new()
        };

        return await PostAsync(newSpool);
    }
}
