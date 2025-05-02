using Microsoft.Extensions.Logging;

namespace Gateways;

internal class VendorSpoolmanEndpoint(SpoolmanConfiguration configuration, ILogger<SpoolmanEndpoint<Vendor>> logger) : SpoolmanEndpoint<Vendor>(configuration, logger), IVendorEndpoint
{
    protected override string Endpoint => "vendor";

    // Get or create a vendor
    public async Task<Vendor> GetOrCreate(string name)
    {
        var vendorResponse = await GetAllAsync($"name={name}");

        Vendor? vendor;
        if (vendorResponse != null && vendorResponse.Any())
            vendor = vendorResponse.First();
        else
        {
            var newVendor = new Vendor
            {
                Name = name
            };

            vendor = await PostAsync(newVendor);
        }

        return vendor ?? throw new InvalidOperationException("Failed to create or retrieve vendor.");
    }
}
