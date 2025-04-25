using Gateways;

namespace Domain;

internal sealed class GetAllAMSUseCase(HomeAssistantClient homeAssistantClient) : IUseCase<GetAllAMSInput>
{
    public async Task<IOutput> ExecuteAsync(GetAllAMSInput input)
    {
        var amsEntities = await homeAssistantClient.GetAmsInfoAsync();

        var externalSpool = await homeAssistantClient.GetExternalSpoolAsync();

        return new GetAllAMSOutput(amsEntities, externalSpool);
    }
}