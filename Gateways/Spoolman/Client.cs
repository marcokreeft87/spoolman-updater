

namespace Gateways;

public class SpoolmanClient(IHealthEndpoint healthEndpoint, ISpoolEndpoint spoolEndpoint, IFieldEndpoint fieldEndpoint)
{
    public async Task<List<Spool>> GetAllAsync() => await spoolEndpoint.GetAllAsync();

    public async Task<bool> UseSpoolWeightAsync(int spoolId, float usedWeight) =>
        await spoolEndpoint.UseSpoolWeight(spoolId, usedWeight);

    public async Task<Spool?> GetSpoolByBrandAndColorAsync(string brand, string material, string color, string activeTrayId, string tagUid) =>
        await spoolEndpoint.GetOrCreateSpool(brand, material, color, activeTrayId, tagUid);

    public async Task<bool> SetActiveTray(int spoolId, string activeTrayId) =>
        await spoolEndpoint.SetActiveTray(spoolId, activeTrayId);

    public async Task<List<Spool>> GetCurrentSpoolsInTray(string trayId) =>
        await spoolEndpoint.GetCurrentSpoolsInTray(trayId);

    public async Task<bool> CheckHealthAsync() =>
        await healthEndpoint.CheckHealthAsync();

    public async Task CheckFieldExistence() =>
        await fieldEndpoint.CheckFieldExistence();

    public async Task<Spool> GetByIdAsync(int spoolId) => await spoolEndpoint.GetByIdAsync(spoolId);
}
