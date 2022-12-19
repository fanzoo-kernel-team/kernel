namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWorkFactory : IDisposable, IAsyncDisposable
    {
        IUnitOfWork Open();

        IUnitOfWork Current { get; }

        bool CanOpen { get; }
    }
}
