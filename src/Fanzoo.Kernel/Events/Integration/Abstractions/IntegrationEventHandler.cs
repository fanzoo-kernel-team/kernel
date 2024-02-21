using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Logging;
using Serilog;

namespace Fanzoo.Kernel.Events.Integration
{
    public interface IIntegrationEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent { }

    public abstract class IntegrationEventHandler<TEvent> : IIntegrationEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EventDispatcher _eventDispatcher;

        protected IntegrationEventHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

            ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

            _unitOfWork = unitOfWorkFactory.Open();
        }

        protected ILogger Logger => Log.Logger;

        public async ValueTask HandleAsync(TEvent @event)
        {
            Logger.EventInformation<TEvent>("Begin ---------->");

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
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();

                Logger.EventException<TEvent>(e);
            }
            finally
            {
                Logger.EventInformation<TEvent>("<---------- End");
            }
        }

        protected abstract ValueTask OnHandleAsync(TEvent @event);
    }
}
