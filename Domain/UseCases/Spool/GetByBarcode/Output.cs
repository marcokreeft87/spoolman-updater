using Gateways;

namespace Domain;

internal class GetByBarcodeOutput : IOutput
{
    public GetByBarcodeOutput(List<Spool> spools)
    {
        Spools = spools;
    }

    public List<Spool> Spools { get; }
}