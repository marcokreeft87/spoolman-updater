using Gateways;

namespace Domain;

internal sealed class GetAllTraysUseCase(HomeAssistantClient homeAssistantClient) : IUseCase<GetAllTraysInput>
{
    public async Task<IOutput> ExecuteAsync(GetAllTraysInput input)
    {
        var trays = await homeAssistantClient.GetAllTrayInfoAsync();

        return new GetAllTraysOutput(trays);
    }
}