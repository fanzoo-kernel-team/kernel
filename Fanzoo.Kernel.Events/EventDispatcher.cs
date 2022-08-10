using Fanzoo.Kernel.Events.Domain;
using Fanzoo.Kernel.Events.Integration;

namespace Fanzoo.Kernel.Events
{
    public sealed class EventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Queue<IEvent> _deferredEvents;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _deferredEvents = new();
        }

        public void QueueIntegrationEvent(IEvent @event) => _deferredEvents.Enqueue(@event);

        public void QueueIntegrationEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _deferredEvents.Enqueue(@event);
            }
        }

        public async ValueTask DispatchDeferredIntegrationEventsAsync()
        {
            while (_deferredEvents.Count > 0)
            {
                var @event = _deferredEvents.Dequeue();

                await DispatchImmediateAsync(@event, typeof(IIntegrationEventHandler<>));
            }
        }

        public async ValueTask DispatchDomainEventAsync(IEvent @event) => await DispatchImmediateAsync(@event, typeof(IDomainEventHandler<>));

        public async ValueTask DispatchIntegrationEventAsync(IEvent @event) => await DispatchImmediateAsync(@event, typeof(IIntegrationEventHandler<>));

        private async ValueTask DispatchImmediateAsync(IEvent @event, Type eventHandlerType)
        {
            static bool IsHandlerInterface(Type type, Type handlerType) => type.IsGenericType &&
                        type.GetGenericTypeDefinition() == handlerType;

            var handlerTypes = _serviceProvider.GetConfiguredServiceTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetInterfaces().Any(i => IsHandlerInterface(i, eventHandlerType)))
                    .Select(t => (Type: t, Interface: t.GetInterfaces().Single(i => IsHandlerInterface(i, eventHandlerType))))
                        .Where(i => i.Interface.GenericTypeArguments.Any(t => t == @event.GetType()));

            foreach (var handlerType in handlerTypes)
            {
                dynamic eventHandler = _serviceProvider
                    .GetService(handlerType.Type)!;

                await eventHandler.HandleAsync((dynamic)@event);
            }
        }
    }
}
