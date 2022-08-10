namespace Fanzoo.Kernel.Data.Queries.Domain
{
    public abstract class SingleOrDefaultQuerySpecification<TEntity> : IQuerySpecification<TEntity> where TEntity : class, IAggregateRoot
    {
        internal async ValueTask<TEntity?> ExecuteAsync(IQueryable<TEntity> query) => await OnExecuteAsync(query);

        protected abstract ValueTask<TEntity?> OnExecuteAsync(IQueryable<TEntity> query);
    }
}
