using Gateways;

namespace Domain;

internal class GetAllSpoolsOutput : IOutput
{
    public GetAllSpoolsOutput(List<Spool> spools)
    {
        Spools = spools;
    }

    public List<Spool> Spools { get; }
}