namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateStandAloneRepository<TEntity> : NHibernateRepositoryCore<TEntity>, IStandAloneRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        private readonly ITransaction _transaction;

        public NHibernateStandAloneRepository(ISession session) : base(session)
        {
            _session.FlushMode = FlushMode.Commit;

            _transaction = _session.BeginTransaction();
        }

        public async ValueTask SaveAsync() => await _transaction.CommitAsync();

        public void Dispose()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();

            _session.Dispose();
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
