namespace Fanzoo.Kernel.Domain.Entities.Guid
{
    public abstract class Entity<TIdentifier> : Entity<TIdentifier, System.Guid>
        where TIdentifier : IdentifierValue<System.Guid>, new()
    {

    }
}
