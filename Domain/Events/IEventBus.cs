namespace Domain;

public interface IEventBus
{
    Task RaiseEventAsync(params IEvent[] raisedEvents);
}
