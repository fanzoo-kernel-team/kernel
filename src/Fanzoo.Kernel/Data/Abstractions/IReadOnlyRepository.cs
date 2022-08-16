using Fanzoo.Kernel.Data.Queries.Domain;

namespace Fanzoo.Kernel.Data
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        ValueTask<IEnumerable<TEntity>> FindAllAsync<TQuery>(TQuery query) where TQuery : AllQuerySpecification<TEntity>;

        ValueTask<TEntity> FindSingleAsync<TQuery>(TQuery query) where TQuery : SingleQuerySpecification<TEntity>;

        ValueTask<TEntity?> FindSingleOrDefaultAsync<TQuery>(TQuery query) where TQuery : SingleOrDefaultQuerySpecification<TEntity>;

        ValueTask<TReturnValue> GetScalarAsync<TQuery, TReturnValue>(TQuery query) where TQuery : ScalarQuerySpecification<TEntity, TReturnValue>;

        public ValueTask<TEntity> LoadAsync(object id); //TODO: make based on identifier

        public ValueTask<TEntity?> FindAsync(object id);  //TODO: make based on identifier
    }
}
