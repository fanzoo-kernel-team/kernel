namespace Fanzoo.Kernel.Data.Queries.Domain
{
    public abstract class AllQuerySpecification<TEntity> : IQuerySpecification<TEntity> where TEntity : class, IAggregateRoot
    {
        internal async ValueTask<IEnumerable<TEntity>> ExecuteAsync(IQueryable<TEntity> query) => await OnExecuteAsync(query);

        protected abstract ValueTask<IEnumerable<TEntity>> OnExecuteAsync(IQueryable<TEntity> query);
    }
}
