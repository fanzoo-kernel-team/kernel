namespace Fanzoo.Kernel.Events.Domain
{
    public interface IDomainEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {

    }

    public abstract class DomainEventHandler<TEvent> : EventHandler<TEvent>, IDomainEventHandler<TEvent> where TEvent : IEvent
    {
        protected override ValueTask OnErrorAsync(Exception e) => throw e;
    }
}
