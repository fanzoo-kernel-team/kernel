namespace Fanzoo.Kernel.Events.Integration
{
    public interface IIntegrationEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {

    }
}
