#pragma warning disable S2326 // Unused type parameters should be removed

namespace Fanzoo.Kernel.Data.Queries.Domain
{
    public interface IQuerySpecification<TEntity> where TEntity : class, IAggregateRoot
    {

    }
}
