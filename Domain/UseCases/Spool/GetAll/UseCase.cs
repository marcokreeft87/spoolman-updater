using Gateways;

namespace Domain;

internal sealed class GetAllSpoolsUseCase(SpoolmanClient spoolmanClient) : IUseCase<GetAllSpoolsInput>
{
    public async Task<IOutput> ExecuteAsync(GetAllSpoolsInput input)
    {
        var spools = await spoolmanClient.GetAllAsync();

        return new GetAllSpoolsOutput(spools);
    }
}