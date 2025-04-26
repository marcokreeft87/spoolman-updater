namespace Gateways;

internal static class FilamentQueryConstants
{
    private static string Prefix = "filament";

    public static string FilamentVendorName => $"{Prefix}.{VendorName}";

    public static string FilamentMaterial => $"{Prefix}.{Material}";

    public static string ColorHex = "color_hex";
    public static string Material = "material";

    public static string VendorName = "vendor.name";

    public static string ActiveTrayId = "active_tray";
    public static string TagUid = "tag";
}