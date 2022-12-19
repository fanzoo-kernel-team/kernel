namespace Fanzoo.Kernel.Defaults.Domain.Entities
{
    public abstract class AggregateRoot<TIdentifier> : AggregateRoot<TIdentifier, Guid>
        where TIdentifier : IdentifierValue<Guid>, new()
    {

    }
}
