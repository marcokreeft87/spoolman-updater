namespace Gateways;

public class SpoolmanConfiguration
{
    public string Url { get; set; } = string.Empty;

    public string PrinterId { get; set; } = string.Empty;

    public List<VendorMapping> VendorMappings { get; set; } = new List<VendorMapping>();
}
