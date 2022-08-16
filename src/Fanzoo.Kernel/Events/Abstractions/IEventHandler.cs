namespace Fanzoo.Kernel.Events
{
    public interface IEventHandler<in IEvent>
    {
        ValueTask HandleAsync(IEvent @event);
    }
}
