namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public interface IBlobStorageSecurityTokenPersistenceService
    {
        ValueTask ClearCurrentContainerSecurityTokenAsync();

        ValueTask<string> GetCurrentContainerSecurityTokenAsync();
    }
}
