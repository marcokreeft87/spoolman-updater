using System.Text.Json.Serialization;

namespace Gateways;

public class HaEntity
{
    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; }
    [JsonPropertyName("device_id")]
    public string DeviceId { get; set; }
    public string State { get; set; }
    public Dictionary<string, object> Attributes { get; set; }
}
