using NHibernate.Linq;

namespace Fanzoo.Kernel.Data
{
    public abstract class NHibernateRepositoryCore<TAggregateRoot, TIdentifier, TPrimitive> : IRepository<TAggregateRoot, TIdentifier, TPrimitive>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TIdentifier, TPrimitive>, ITrackableEntity
        where TIdentifier : notnull, IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        protected NHibernateRepositoryCore(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));

            _unitOfWorkFactory.Open();
        }

        public async ValueTask<TAggregateRoot> LoadAsync(TIdentifier id) => await _unitOfWorkFactory.Current.GetContext().LoadAsync<TAggregateRoot, TIdentifier>(id);

        public async ValueTask AddAsync(TAggregateRoot entity)
        {
            if (!entity.IsTransient)
            {
                throw new InvalidOperationException("entity must be transient to add");
            }

            await _unitOfWorkFactory.Current.GetContext().AddAsync(entity);
        }

        public async ValueTask DeleteAsync(TAggregateRoot entity)
        {
            if (entity.IsTransient)
            {
                throw new InvalidOperationException("transient entity cannot be deleted");
            }

            await _unitOfWorkFactory.Current.GetContext().DeleteAsync(entity);
        }

        public async ValueTask DeleteAsync(TIdentifier id) => await Query.Where(e => e.Id == id).DeleteAsync();

        protected IQueryable<TAggregateRoot> Query => _unitOfWorkFactory.Current.GetContext().Query<TAggregateRoot>();
    }
}
