namespace Gateways;

public class HomeAssistantConfiguration
{
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string ExternalSpoolEntity { get; set; } = string.Empty;
    public string[] AMSEntities { get; set; } = Array.Empty<string>();
    public string[] TrayEntities { get; set;} = Array.Empty<string>();
}
