namespace Domain;

internal class GetSettingsOutput : IOutput
{
    public UpdaterConfiguration Configuration { get; set; }

	public GetSettingsOutput(UpdaterConfiguration configuration)
    {
        Configuration = configuration;
    }
}