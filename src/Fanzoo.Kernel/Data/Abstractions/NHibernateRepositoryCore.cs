using Fanzoo.Kernel.Data.Queries.Domain;
using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public abstract class NHibernateRepositoryCore<TEntity> where TEntity : class, IAggregateRoot
    {
        protected readonly ISession _session;

        protected NHibernateRepositoryCore(ISession session)
        {
            _session = session;
        }

        public async ValueTask<TEntity> LoadAsync(object id) => await _session.LoadAsync<TEntity>(id);

        public async ValueTask<TEntity?> FindAsync(object id) => await _session.GetAsync<TEntity>(id); //TODO: revisit

        public async ValueTask AddAsync(TEntity entity) => await _session.SaveAsync(entity); //TODO: this should check if it's transient first!

        public async ValueTask UpdateAsync(TEntity entity) => await _session.UpdateAsync(entity); //TODO: this needs to go away

        public async ValueTask DeleteAsync(TEntity entity) => await _session.DeleteAsync(entity);

        public async ValueTask<TEntity?> FindSingleOrDefaultAsync<TQuery>(TQuery query) where TQuery : SingleOrDefaultQuerySpecification<TEntity> => await query.ExecuteAsync(_session.Query<TEntity>());

        public async ValueTask<TEntity> FindSingleAsync<TQuery>(TQuery query) where TQuery : SingleQuerySpecification<TEntity> => await query.ExecuteAsync(_session.Query<TEntity>());

        public async ValueTask<IEnumerable<TEntity>> FindAllAsync<TQuery>(TQuery query) where TQuery : AllQuerySpecification<TEntity> => await query.ExecuteAsync(_session.Query<TEntity>());

        public async ValueTask<TReturnValue> GetScalarAsync<TQuery, TReturnValue>(TQuery query) where TQuery : ScalarQuerySpecification<TEntity, TReturnValue> => await query.ExecuteAsync(_session.Query<TEntity>());
    }
}
