namespace Gateways;

internal class VendorSpoolmanEndpoint(SpoolmanConfiguration configuration) : SpoolmanEndpoint<Vendor>(configuration), IVendorEndpoint
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
