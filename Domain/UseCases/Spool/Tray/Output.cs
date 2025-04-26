using Gateways;

namespace Domain;

internal record UpdateTrayOutput(Spool Spool) : IOutput;