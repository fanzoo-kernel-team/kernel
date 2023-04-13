namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWorkFactory : IDisposable, IAsyncDisposable
    {
        IUnitOfWork Open();

        void Close();

        IUnitOfWork Current { get; }

        bool HasUnitOfWork { get; }

        bool CanOpen { get; }
    }
}
