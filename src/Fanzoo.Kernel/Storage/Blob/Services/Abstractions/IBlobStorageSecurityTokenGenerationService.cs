namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public interface IBlobStorageSecurityTokenGenerationService
    {
        int SecurityTokenDurationMinutes { get; }

        ValueTask<string> GenerateContainerSecurityTokenAsync();

        ValueTask<string> GenerateBlobSecurityTokenAsync(string blobPathName);
    }
}
