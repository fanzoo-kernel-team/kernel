using CSharpFunctionalExtensions;
using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Domain.Entities;
using Fanzoo.Kernel.Events;
using Serilog;

namespace Fanzoo.Kernel.Commands;

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    private readonly ILogger _logger;

    private readonly EventDispatcher _eventDispatcher;

    private readonly IScopedUnitOfWork _unitOfWork;

    protected CommandHandler(IScopedUnitOfWork unitOfWork, EventDispatcher eventDispatcher)
    {
        _unitOfWork = unitOfWork;
        _eventDispatcher = eventDispatcher;
        _logger = Log.ForContext<CommandHandler<TCommand, TResult>>();
    }

    protected IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot => _unitOfWork.Repository<TEntity>();

    public async Task<CommandResult<TResult>> HandleAsync(TCommand command)
    {
        try
        {
            var result = await OnHandleAsync(command);

            if (result.IsSuccessful)
            {
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

                await _eventDispatcher.DispatchDeferredIntegrationEventsAsync();
            }

            return result;
        }
        catch (ResultFailureException<Error> e)
        {
            await _unitOfWork.RollbackAsync();

            return CommandResult<TResult>.Fail(e.Error);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();

            return CommandResult<TResult>.Fail(e);
        }
    }

    protected void PublishIntegrationEvent(IEvent @event) =>
        //can we publish the same event twice? (Probably can with different values)

        _eventDispatcher.QueueIntegrationEvent(@event);

    protected abstract Task<CommandResult<TResult>> OnHandleAsync(TCommand command);

}

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ILogger _logger;

    private readonly EventDispatcher _eventDispatcher;

    private readonly IScopedUnitOfWork _unitOfWork;

    protected CommandHandler(IScopedUnitOfWork unitOfWork, EventDispatcher eventDispatcher)
    {
        _unitOfWork = unitOfWork;
        _eventDispatcher = eventDispatcher;
        _logger = Log.ForContext<CommandHandler<TCommand>>();
    }

    protected IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot => _unitOfWork.Repository<TEntity>();

    public async Task<CommandResult> HandleAsync(TCommand command)
    {
        try
        {
            var result = await OnHandleAsync(command);

            if (result.IsSuccessful)
            {
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

                await _eventDispatcher.DispatchDeferredIntegrationEventsAsync();
            }

            return result;
        }
        catch (ResultFailureException<Error> e)
        {
            await _unitOfWork.RollbackAsync();

            return CommandResult.Fail(e.Error);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();

            return CommandResult.Fail(e);
        }
    }

    protected void PublishIntegrationEvent(IEvent @event) => _eventDispatcher.QueueIntegrationEvent(@event);

    protected abstract Task<CommandResult> OnHandleAsync(TCommand command);
}

