namespace Gateways;

public class HomeAssistantState
{
    public string State { get; set; } = string.Empty;
    public TrayInfo Attributes { get; set; } = new();
}
