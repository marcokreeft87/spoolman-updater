namespace Gateways;

public class AMSEntity
{
    public string Id { get; set; } = string.Empty;

    public List<TrayInfo> Trays { get; set; } = new();
}