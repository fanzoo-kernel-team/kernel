namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public interface IBlobStorageSecurityTokenPersistenceService
    {
        ValueTask<string> GetCurrentContainerSecurityTokenAsync();
    }
}
