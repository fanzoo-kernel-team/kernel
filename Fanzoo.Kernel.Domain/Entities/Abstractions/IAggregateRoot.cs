using Fanzoo.Kernel.Events;

namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IAggregateRoot
    {
        ICollection<IEvent> Events { get; }
    }
}
