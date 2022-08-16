using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateRepository<TEntity> : NHibernateRepositoryCore<TEntity>, IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        public NHibernateRepository(ISession session) : base(session) { }
    }
}
