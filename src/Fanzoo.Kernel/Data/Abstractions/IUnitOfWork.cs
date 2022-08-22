namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWork
    {
        bool IsDirty { get; }

        bool IsClosed { get; }

        bool WasCommitted { get; }

        bool WasRolledBack { get; }

        IUnitOfWorkContext GetContext();

        ValueTask CommitAsync();

        ValueTask RollbackAsync();
    }
}
