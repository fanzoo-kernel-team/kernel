namespace Fanzoo.Kernel.Data.Queries.Domain
{
    public abstract class ScalarQuerySpecification<TEntity, TReturnValue> : IQuerySpecification<TEntity> where TEntity : class, IAggregateRoot
    {
        internal async ValueTask<TReturnValue> ExecuteAsync(IQueryable<TEntity> query) => await OnExecuteAsync(query);

        protected abstract ValueTask<TReturnValue> OnExecuteAsync(IQueryable<TEntity> query);
    }
}
