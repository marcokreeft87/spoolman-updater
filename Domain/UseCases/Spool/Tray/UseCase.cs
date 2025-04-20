using Gateways;

namespace Domain;

internal sealed class UpdateTrayUseCase(SpoolmanClient spoolmanClient) : IUseCase<UpdateTrayInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateTrayInput input)
    {
        var spool = await spoolmanClient.SetActiveTray(input.SpoolId, input.ActiveTrayId);

        return new UpdateTrayOutput(spool != null);
    }
}