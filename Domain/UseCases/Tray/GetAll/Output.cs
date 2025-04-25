using Gateways;

namespace Domain;

internal class GetAllAMSOutput : IOutput
{
    public GetAllAMSOutput(List<AMSEntity> amsEntities, TrayInfo extranalTray)
    {
        AMSEntities = amsEntities;
        ExternalSpoolEntity = extranalTray;
    }

    public List<AMSEntity> AMSEntities { get; }

    public TrayInfo? ExternalSpoolEntity { get; }
}