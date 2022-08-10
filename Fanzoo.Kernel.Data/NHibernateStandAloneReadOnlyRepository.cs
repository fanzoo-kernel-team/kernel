namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateStandAloneReadOnlyRepository<TEntity> : NHibernateRepositoryCore<TEntity>, IStandAloneReadOnlyRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        public NHibernateStandAloneReadOnlyRepository(ISession session) : base(session)
        {
            _session.DefaultReadOnly = true;
        }

        public void Dispose() => _session.Dispose();

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    }
}
