using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Data;

namespace Fanzoo.Kernel.Events.Integration
{

    public abstract class IntegrationEventHandler<TEvent> : IIntegrationEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EventDispatcher _eventDispatcher;

        protected IntegrationEventHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

            if (unitOfWorkFactory is null)
            {
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            }

            _unitOfWork = unitOfWorkFactory.Open();
        }

        public async ValueTask HandleAsync(TEvent @event)
        {
            try
            {
                await OnHandleAsync(@event);

                //handle cascading domain events
                var entities = _unitOfWork.GetEntitiesWithEvents().ToArray();

                while (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        foreach (var domainEvent in entity.Events)
                        {
                            await _eventDispatcher.DispatchDomainEventAsync(domainEvent);
                        }

                        _eventDispatcher.QueueIntegrationEvents(entity.Events);

                        entity.Events.Clear();
                    }

                    entities = _unitOfWork.GetEntitiesWithEvents().ToArray();
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
            }
        }

        protected abstract ValueTask OnHandleAsync(TEvent @event);
    }
}
