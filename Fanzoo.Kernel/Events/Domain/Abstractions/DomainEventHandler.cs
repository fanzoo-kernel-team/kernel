using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Domain.Entities;

namespace Fanzoo.Kernel.Events.Domain
{

    public abstract class DomainEventHandler<TEvent> : EventHandler<TEvent>, IDomainEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IScopedUnitOfWork _unitOfWork;

        protected DomainEventHandler(IScopedUnitOfWork unitOfWork) : base()
        {
            _unitOfWork = unitOfWork;
        }

        protected IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot => _unitOfWork.Repository<TEntity>();

        protected override ValueTask OnErrorAsync(Exception e) =>
            //let the error bubble up

            throw e;
    }
}
