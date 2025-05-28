namespace Domain;

internal sealed class GetSettingsUseCase(UpdaterConfiguration configuration) : IUseCase<GetSettingsInput>
{
    public async Task<IOutput> ExecuteAsync(GetSettingsInput input)
    {
        return new GetSettingsOutput(configuration);
    }
}