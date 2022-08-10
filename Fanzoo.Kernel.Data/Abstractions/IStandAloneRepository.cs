namespace Fanzoo.Kernel.Data
{
    public interface IStandAloneRepository<TEntity> : IRepository<TEntity>, IDisposable, IAsyncDisposable where TEntity : class, IAggregateRoot
    {
        public ValueTask SaveAsync();

    }
}
