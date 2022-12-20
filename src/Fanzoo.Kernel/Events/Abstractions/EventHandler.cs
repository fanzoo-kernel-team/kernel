using Fanzoo.Kernel.Logging;
using Serilog;

namespace Fanzoo.Kernel.Events
{
    public interface IEventHandler<in IEvent>
    {
        ValueTask HandleAsync(IEvent @event);
    }

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        protected ILogger Logger => Log.Logger;

        public async ValueTask HandleAsync(TEvent @event)
        {
            try
            {
                Logger.EventInformation<TEvent>("Begin ---------->");

                await OnHandleAsync(@event);

                await OnSuccessAsync();
            }
            catch (Exception e)
            {
                await OnErrorAsync(e);

                Logger.EventException<TEvent>(e);
            }
            finally
            {
                Logger.EventInformation<TEvent>("<---------- End");
            }
        }

        protected abstract ValueTask OnHandleAsync(TEvent @event);

        protected virtual ValueTask OnSuccessAsync() => ValueTask.CompletedTask;

        protected virtual ValueTask OnErrorAsync(Exception e) => ValueTask.CompletedTask;
    }
}
