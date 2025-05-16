using Microsoft.Extensions.DependencyInjection;

namespace Domain;

internal interface IEventHandler
{
    Task HandleAsync(IEvent raisedEvent);
}

internal sealed class EventHandler<TEvent>(IServiceProvider serviceProvider) : IEventHandler where TEvent : class, IEvent
{
    private readonly IEventHandler<TEvent> handlerService =
        serviceProvider
            .GetService<IEventHandler<TEvent>>() ?? throw new InvalidOperationException($"No handler found for event type {typeof(TEvent).Name}");

    public async Task HandleAsync(IEvent raisedEvent) =>
        await handlerService.HandleAsync((TEvent)raisedEvent);
}