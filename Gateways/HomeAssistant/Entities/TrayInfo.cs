using System.Text.Json.Serialization;

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


public class BambuServiceRequest
{
    [JsonPropertyName("entity_id")]
    public string[] EntityId { get; set; }
    [JsonPropertyName("tray_info_idx")]
    public string TrayInfoIdx { get; set; }
    [JsonPropertyName("tray_color")]
    public string TrayColor { get; set; }
    [JsonPropertyName("tray_type")]
    public string TrayType { get; set; }
    [JsonPropertyName("nozzle_temp_min")]
    public int NozzleTempMin { get; set; }
    [JsonPropertyName("nozzle_temp_max")]
    public int NozzleTempMax { get; set; }
}
