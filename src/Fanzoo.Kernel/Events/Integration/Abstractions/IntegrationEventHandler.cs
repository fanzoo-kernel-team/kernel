using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Domain.Entities;

namespace Fanzoo.Kernel.Events.Integration
{

    public abstract class IntegrationEventHandler<TEvent> : EventHandler<TEvent>, IIntegrationEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IReadOnlyUnitOfWork _unitOfWork;
        protected IntegrationEventHandler(IReadOnlyUnitOfWork unitOfWork) : base()
        {
            _unitOfWork = unitOfWork;
        }

        protected IReadOnlyRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot => _unitOfWork.Repository<TEntity>();

        protected ValueTask CommitAsync() => throw new NotImplementedException();

        protected ValueTask RollbackAsync() => throw new NotImplementedException();

        protected override ValueTask OnSuccessAsync() => ValueTask.CompletedTask;

        protected override ValueTask OnErrorAsync(Exception e) => ValueTask.CompletedTask;
    }
}
