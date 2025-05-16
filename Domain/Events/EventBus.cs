using Microsoft.Extensions.DependencyInjection;

namespace Domain;

internal sealed class EventBus(IServiceScopeFactory serviceScopeFactory) : IEventBus
{
    public async Task RaiseEventAsync(params IEvent[] raisedEvents)
    {
        foreach (var raisedEvent in raisedEvents)
        {
            await HandleEventAsync(raisedEvent);
        }
    }

    private async Task HandleEventAsync(IEvent raisedEvent)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();

        var handler = Activator
            .CreateInstance(typeof(EventHandler<>)
                .MakeGenericType(raisedEvent.GetType()), scope.ServiceProvider) as IEventHandler;

        await handler.HandleAsync(raisedEvent);
    }
}
