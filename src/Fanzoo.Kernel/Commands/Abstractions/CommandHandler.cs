using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Events;

namespace Fanzoo.Kernel.Commands;

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventDispatcher _eventDispatcher;

    protected CommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

        if (unitOfWorkFactory is null)
        {
            throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        _unitOfWork = unitOfWorkFactory.Open();
    }

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

                await _unitOfWork.CommitAsync(); //this closes the UnitOfWork

                await _eventDispatcher.DispatchDeferredIntegrationEventsAsync(); //this will open subsequent UnitOfWork's
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

    protected void PublishIntegrationEvent(IEvent @event) => _eventDispatcher.QueueIntegrationEvent(@event);

    protected abstract Task<CommandResult<TResult>> OnHandleAsync(TCommand command);

}

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventDispatcher _eventDispatcher;

    protected CommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

        if (unitOfWorkFactory is null)
        {
            throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        _unitOfWork = unitOfWorkFactory.Open();
    }

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

