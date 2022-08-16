namespace Fanzoo.Kernel.Data
{
    public interface IStandAloneReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>, IDisposable, IAsyncDisposable where TEntity : class, IAggregateRoot
    {

    }
}
