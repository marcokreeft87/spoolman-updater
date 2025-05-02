using Microsoft.Extensions.Logging;

namespace Gateways;

internal class HealthCheckSpoolmanEndpoint : SpoolmanEndpoint<Health>, IHealthEndpoint
{
    protected override string Endpoint => "health";

    public HealthCheckSpoolmanEndpoint(SpoolmanConfiguration configuration, ILogger<SpoolmanEndpoint<Health>> logger) : base(configuration, logger) { }

    public async Task<bool> CheckHealthAsync() =>
        (await HttpClient.GetAsync(Endpoint)).IsSuccessStatusCode;
}