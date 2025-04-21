using Gateways;

namespace Domain;

internal sealed class UpdateAllSpoolsUseCase(HomeAssistantClient homeAssistantClient, SpoolmanClient spoolmanClient) : IUseCase<UpdateAllSpoolsInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateAllSpoolsInput input)
    {
        var trayInfos = await homeAssistantClient.GetAllTrayInfoAsync();

        List<Spool> spools = new();
        foreach (var trayInfo in trayInfos)
        {
            if (trayInfo == null)
                continue;

            string trayName = trayInfo.Name ?? "Unknown"; // Handle null values

            var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(trayName, trayInfo.Type, trayInfo.Color, "", trayInfo.TagUid);

            spools.Add(spool);
        }

        return new UpdateAllSpoolsOutput(spools);
    }
}