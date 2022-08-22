namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        bool IsDirty { get; }

        bool WasCommitted { get; }

        bool WasRolledBack { get; }

        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot; //TODO: remove when repositories are freed

        ValueTask CommitAsync();

        ValueTask RollbackAsync();
    }
}
