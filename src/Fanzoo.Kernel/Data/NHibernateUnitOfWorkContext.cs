using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateUnitOfWorkContext : IUnitOfWorkContext
    {
        private readonly ISession _session;

        public NHibernateUnitOfWorkContext(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async ValueTask<TEntity> LoadAsync<TEntity, TIdentifier>(TIdentifier identifier)
            where TEntity : class
            where TIdentifier : notnull =>
                await _session.LoadAsync<TEntity>(identifier);

        public IQueryable<TEntity> Query<TEntity>() => _session.Query<TEntity>();

        public async ValueTask AddAsync<TEntity>(TEntity entity) where TEntity : class => await _session.SaveAsync(entity);

        public async ValueTask UpdateAsync<TEntity>(TEntity entity) where TEntity : class => await _session.UpdateAsync(entity);

        public async ValueTask DeleteAsync<TEntity>(TEntity entity) where TEntity : class => await _session.DeleteAsync(entity);
    }
}
