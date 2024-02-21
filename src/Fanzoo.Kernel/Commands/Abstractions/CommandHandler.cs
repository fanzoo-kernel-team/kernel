using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Events;
using Fanzoo.Kernel.Logging;
using Serilog;

namespace Fanzoo.Kernel.Commands;

public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventDispatcher _eventDispatcher;

    private readonly IList<IEvent> _domainEvents = [];

    protected CommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

        if (unitOfWorkFactory is null)
        {
            throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        _unitOfWork = unitOfWorkFactory.Open();
    }

    protected ILogger Logger => Log.Logger;

    public async Task<CommandResult<TResult>> HandleAsync(TCommand command)
    {
        Logger.CommandInformation<TCommand>("Begin ---------->");

        try
        {
            var result = await OnHandleAsync(command);

            if (result.IsSuccessful)
            {
                //handle domain events from the command handler
                foreach (var domainEvent in _domainEvents)
                {
                    await _eventDispatcher.DispatchDomainEventAsync(domainEvent);
                }

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
        catch (KernelErrorException e)
        {
            await _unitOfWork.RollbackAsync();

            Logger.CommandException<TCommand>(e);

            return CommandResult<TResult>.Fail(e);
        }
        finally
        {
            Logger.CommandInformation<TCommand>("<---------- End");
        }
    }

    protected void PublishDomainEvent(IEvent @event) => _domainEvents.Add(@event);

    protected void PublishIntegrationEvent(IEvent @event) => _eventDispatcher.QueueIntegrationEvent(@event);

    protected abstract Task<CommandResult<TResult>> OnHandleAsync(TCommand command);

}

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventDispatcher _eventDispatcher;

    private readonly IList<IEvent> _domainEvents = [];

    protected CommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));

        if (unitOfWorkFactory is null)
        {
            throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        _unitOfWork = unitOfWorkFactory.Open();
    }

    protected ILogger Logger => Log.Logger;

    public async Task<CommandResult> HandleAsync(TCommand command)
    {
        Logger.CommandInformation<TCommand>("Begin ---------->");

        try
        {
            var result = await OnHandleAsync(command);

            if (result.IsSuccessful)
            {
                //handle domain events from the command handler
                foreach (var domainEvent in _domainEvents)
                {
                    await _eventDispatcher.DispatchDomainEventAsync(domainEvent);
                }

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
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();

            Logger.CommandException<TCommand>(e);

            return CommandResult.Fail(e);
        }
        finally
        {
            Logger.CommandInformation<TCommand>("<---------- End");
        }
    }

    protected void PublishDomainEvent(IEvent @event) => _domainEvents.Add(@event);

    protected void PublishIntegrationEvent(IEvent @event) => _eventDispatcher.QueueIntegrationEvent(@event);

    protected abstract Task<CommandResult> OnHandleAsync(TCommand command);
}

