namespace Gateways;

public interface ISpoolmanEndpoint<TSpoolmanEntity>
    where TSpoolmanEntity : class
{
}

public interface IVendorEndpoint
{
    Task<Vendor> GetOrCreate(string name);
}

public interface IFilamentEndpoint
{
    Task<Filament> GetOrCreate(Vendor vendor, string color, string material);
}

public interface ISpoolEndpoint
{
    Task<Spool> GetOrCreateSpool(string vendorName, string material, string color, string activeTrayId, string tagUid);

    Task<bool> UseSpoolWeight(int spoolId, float usedWeight);

    Task<bool> SetActiveTray(int spoolId, string activeTrayId);

    Task<List<Spool>> GetCurrentSpoolsInTray(string trayId);

    Task<List<Spool>> GetAllAsync();

    Task<Spool> GetByIdAsync(int spoolId);
}

public interface IFieldEndpoint
{
    Task<bool> CheckFieldExistence();
}
