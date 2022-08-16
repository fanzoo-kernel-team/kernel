using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateUnitOfWork : IScopedUnitOfWork
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;

        private readonly Dictionary<Type, object> _repositories = new();

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;

            _session.FlushMode = FlushMode.Commit;

            _transaction = _session.BeginTransaction();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity> ?? throw new InvalidOperationException("Repository not found.");
            }

            var respository = new NHibernateRepository<TEntity>(_session);

            _repositories.Add(typeof(TEntity), respository);

            return respository;
        }

        public bool IsDirty => _session.IsDirty();

        public bool WasCommitted => _transaction.WasCommitted;

        public bool WasRolledBack => _transaction.WasRolledBack;

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

        public ValueTask DisposeAsync() => ValueTask.CompletedTask; //OK to call this, nothing happens
    }
}
