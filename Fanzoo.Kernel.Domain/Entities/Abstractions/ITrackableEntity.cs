namespace Fanzoo.Kernel.Domain.Entities
{
    public interface ITrackableEntity
    {
        bool IsTransient { get; }

        void SetAsLoadedOrSaved();
    }
}
