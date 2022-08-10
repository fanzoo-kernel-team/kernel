namespace Fanzoo.Kernel.Data
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        ValueTask AddAsync(TEntity entity);

        ValueTask UpdateAsync(TEntity entity);

        ValueTask DeleteAsync(TEntity entity);

    }
}
