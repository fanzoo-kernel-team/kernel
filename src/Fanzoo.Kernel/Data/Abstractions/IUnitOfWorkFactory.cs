namespace Fanzoo.Kernel.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Open();

        IUnitOfWork Current { get; }

        bool CanOpen { get; }
    }
}
