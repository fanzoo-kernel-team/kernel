using Serilog;

namespace Fanzoo.Kernel.Events
{
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        protected readonly ILogger _logger;

        protected EventHandler()
        {
            _logger = Log.ForContext<IEvent>();
        }

        public async ValueTask HandleAsync(TEvent @event)
        {
            try
            {
                await OnHandleAsync(@event);

                await OnSuccessAsync();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Event Error");

                await OnErrorAsync(e);
            }
        }

        protected abstract ValueTask OnHandleAsync(TEvent @event);

        protected virtual ValueTask OnSuccessAsync() => ValueTask.CompletedTask;

        protected virtual ValueTask OnErrorAsync(Exception e) => ValueTask.CompletedTask;
    }
}
