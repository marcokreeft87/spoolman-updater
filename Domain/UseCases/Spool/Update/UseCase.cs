using Gateways;

namespace Domain;

internal sealed class UpdateSpoolUseCase(SpoolmanClient spoolmanClient) : IUseCase<UpdateSpoolInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateSpoolInput input)
    {
        var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(input.Name, input.Material, input.Color, input.ActiveTrayId, input.TagUid);
        if (spool == null)
            return new UpdateSpoolOutput(false);

        var success = await spoolmanClient.UseSpoolWeightAsync(spool.Id.Value, input.UsedWeight);

        return new UpdateSpoolOutput(success);
    }
}