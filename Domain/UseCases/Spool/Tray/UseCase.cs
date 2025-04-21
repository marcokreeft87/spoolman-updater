using Gateways;

namespace Domain;

internal sealed class UpdateTrayUseCase(SpoolmanClient spoolmanClient) : IUseCase<UpdateTrayInput>
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

        var spool = await spoolmanClient.SetActiveTray(input.SpoolId, input.ActiveTrayId);

        return new UpdateTrayOutput(spool != null);
    }
}