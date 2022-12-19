namespace Fanzoo.Kernel.Defaults.Domain.Entities
{
    public abstract class Entity<TIdentifier> : Entity<TIdentifier, Guid>
        where TIdentifier : IdentifierValue<Guid>, new()
    {

    }
}
