using Gateways;

namespace Domain;

internal record SpoolUpdatedEvent(Spool Spool, string ActiveTrayId) : IEvent;
