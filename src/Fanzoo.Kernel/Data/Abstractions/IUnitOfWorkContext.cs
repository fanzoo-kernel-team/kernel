namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWorkContext
    {
        ValueTask<TEntity> LoadAsync<TEntity, TIdentifier>(TIdentifier identifier)
            where TEntity : class
            where TIdentifier : notnull;

        ValueTask AddAsync<TEntity>(TEntity entity) where TEntity : class;

        ValueTask UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        ValueTask DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
    }

    public interface IQueryableUnitOfWorkContext
    {
        IQueryable<TEntity> Query<TEntity>();
    }
}
