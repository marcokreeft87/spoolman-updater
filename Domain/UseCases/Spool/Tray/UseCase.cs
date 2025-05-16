using Gateways;

namespace Domain;

internal sealed class UpdateTrayUseCase(SpoolmanClient spoolmanClient, HomeAssistantClient homeassistantClient, IEventBus eventBus) : IUseCase<UpdateTrayInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateTrayInput input)
    { 
        var currentSpools = await spoolmanClient.GetCurrentSpoolsInTray(input.ActiveTrayId);
        
        foreach(var currentSpool in currentSpools)
        {
            if (currentSpool.Id == input.SpoolId)
                continue;

            await spoolmanClient.SetActiveTray(currentSpool.Id.Value, string.Empty);
        }

        if (!await spoolmanClient.SetActiveTray(input.SpoolId, input.ActiveTrayId))
            throw new InvalidOperationException($"Update Spool with ID {input.SpoolId} not successfull.");

        var spool = await spoolmanClient.GetByIdAsync(input.SpoolId);

        await eventBus.RaiseEventAsync(new SpoolUpdatedEvent(spool, input.ActiveTrayId));        

        return new UpdateTrayOutput(spool);
    }
}