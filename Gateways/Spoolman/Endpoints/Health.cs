namespace Gateways;

internal class HealthCheckSpoolmanEndpoint : SpoolmanEndpoint<Health>, IHealthEndpoint
{
    protected override string Endpoint => "health";

    public HealthCheckSpoolmanEndpoint(SpoolmanConfiguration configuration) : base(configuration) { }

    public async Task<bool> CheckHealthAsync() =>
        (await HttpClient.GetAsync(Endpoint)).IsSuccessStatusCode;
}