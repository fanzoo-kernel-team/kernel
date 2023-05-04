namespace Fanzoo.Kernel.Data
{
    public interface IRepository<TAggregateRoot, TIdentifier, TPrimitive>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TIdentifier, TPrimitive>, ITrackableEntity
        where TIdentifier : notnull, IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {
        ValueTask<TAggregateRoot> LoadAsync(TIdentifier id);

        ValueTask AddAsync(TAggregateRoot entity);

        ValueTask DeleteAsync(TAggregateRoot entity);

        ValueTask DeleteAsync(TIdentifier id);
    }
}
