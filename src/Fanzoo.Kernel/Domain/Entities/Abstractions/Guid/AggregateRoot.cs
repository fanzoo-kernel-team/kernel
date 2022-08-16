namespace Fanzoo.Kernel.Domain.Entities.Guid
{
    public abstract class AggregateRoot<TIdentifier> : AggregateRoot<TIdentifier, System.Guid>
        where TIdentifier : IdentifierValue<System.Guid>, new()
    {

    }
}
