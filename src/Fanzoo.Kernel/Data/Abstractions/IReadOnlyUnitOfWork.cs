namespace Fanzoo.Kernel.Data
{
    public interface IReadOnlyUnitOfWork : IDisposable, IAsyncDisposable
    {
        IReadOnlyRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot;
    }
}
