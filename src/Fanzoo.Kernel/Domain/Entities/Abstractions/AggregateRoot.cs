using Fanzoo.Kernel.Events;

namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IAggregateRoot
    {
        ICollection<IEvent> Events { get; }
    }

    public abstract class AggregateRoot<TIdentifier, TPrimitive> : Entity<TIdentifier, TPrimitive>, IAggregateRoot
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
    {
        public ICollection<IEvent> Events { get; init; } = [];

        protected void PublishEvent(IEvent @event) => Events.Add(@event); //can an aggregate raise the same event more than once???
    }
}
