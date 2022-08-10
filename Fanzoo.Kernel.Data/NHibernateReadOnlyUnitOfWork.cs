namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateReadOnlyUnitOfWork : IReadOnlyUnitOfWork
    {
        private readonly ISession _session;

        private readonly Dictionary<Type, object> _repositories = new();

        public NHibernateReadOnlyUnitOfWork(ISession session)
        {
            _session = session;
            _session.DefaultReadOnly = true;
        }

        public IReadOnlyRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity> ?? throw new InvalidOperationException("Repository not found.");
            }

            var respository = new NHibernateRepository<TEntity>(_session);

            _repositories.Add(typeof(TEntity), respository);

            return respository;
        }

        public void Dispose() => _session.Dispose();

        public ValueTask DisposeAsync() => ValueTask.CompletedTask; //OK to call this, nothing happens
    }
}
