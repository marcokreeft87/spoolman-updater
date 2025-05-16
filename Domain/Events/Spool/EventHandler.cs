using Gateways;
using System.Text.Json;

namespace Domain;

internal class EventHandler(HomeAssistantClient homeassistantClient) : IEventHandler<SpoolUpdatedEvent>
{
    public async Task HandleAsync(SpoolUpdatedEvent raisedEvent)
    {
        var spool = raisedEvent.Spool;

        await homeassistantClient.SetPrintTraySpool(
            raisedEvent.ActiveTrayId,
            spool.Filament.ColorHex,
            spool.Filament.Material,
            spool.Filament.Extra.ContainsKey("type") ? JsonSerializer.Deserialize<string>(spool.Filament.Extra["type"]) : string.Empty,
            spool.Filament.ExtruderTemp,
            spool.Filament.ExtruderTemp
        );
    }
}
