using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateUnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));

            _session.FlushMode = FlushMode.Commit;

            _transaction = _session.BeginTransaction();
        }

        public bool IsClosed => _transaction.WasCommitted || _transaction.WasRolledBack;

        public bool IsDirty => _session.IsDirty();

        public bool WasCommitted => _transaction.WasCommitted;

        public bool WasRolledBack => _transaction.WasRolledBack;

        public IUnitOfWorkContext GetContext() => new NHibernateUnitOfWorkContext(_session);

        public async ValueTask CommitAsync()
        {
            if (_transaction.IsActive)
            {
                await _transaction.CommitAsync();
            }
        }

        public async ValueTask RollbackAsync()
        {
            if (_transaction.IsActive)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();

            _session.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
