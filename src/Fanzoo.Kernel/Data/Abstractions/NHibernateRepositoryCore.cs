namespace Fanzoo.Kernel.Data
{
    public abstract class NHibernateRepositoryCore<TAggregateRoot, TIdentifier, TPrimitive> : IRepository<TAggregateRoot, TIdentifier, TPrimitive>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TIdentifier, TPrimitive>, ITrackableEntity
        where TIdentifier : notnull, IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {

        private readonly IUnitOfWorkContext _context;

        protected NHibernateRepositoryCore(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory is null)
            {
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            }

            _context = unitOfWorkFactory.Current.GetContext();
        }

        public async ValueTask<TAggregateRoot> LoadAsync(TIdentifier id) => await _context.LoadAsync<TAggregateRoot, TIdentifier>(id);

        public async ValueTask AddAsync(TAggregateRoot entity)
        {
            if (!entity.IsTransient)
            {
                throw new InvalidOperationException("entity must be transient to add");
            }

            await _context.AddAsync(entity);
        }

        public async ValueTask DeleteAsync(TAggregateRoot entity) => await _context.DeleteAsync(entity);
    }
}
