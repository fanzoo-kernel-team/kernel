namespace Fanzoo.Kernel.Events.Domain
{
    public interface IDomainEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {

    }
}
