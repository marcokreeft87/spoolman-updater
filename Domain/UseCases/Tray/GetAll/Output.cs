using Gateways;

namespace Domain;

internal class GetAllTraysOutput : IOutput
{
    public GetAllTraysOutput(List<TrayInfo> trays)
    {
        Trays = trays;
    }

    public List<TrayInfo> Trays { get; }
}