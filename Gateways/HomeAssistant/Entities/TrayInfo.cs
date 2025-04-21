namespace Gateways;

public class TrayInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string TagUid { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class HomeAssistantState
{
    public string State { get; set; } = string.Empty;
    public TrayInfo Attributes { get; set; } = new();
}
