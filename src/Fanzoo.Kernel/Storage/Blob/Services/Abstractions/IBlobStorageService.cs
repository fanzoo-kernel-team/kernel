namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public enum BlobStorageSecurityTarget
    {
        Container,
        Blob
    }

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"", Justification = "The analyzer doesn't understand bitwise operators")]
    public enum BlobStorageSecurityPermissions
    {
        Read = 1,
        Add = 2,
        Create = 4,
        Write = 8,
        Delete = 16,
        Tag = 32,
        DeleteBlobVersion = 64,
        List = 128,
        Move = 256,
        Execute = 512,
        SetImmutabilityPolicy = 1024,
        PermanentDelete = 2048,
        All = ~0
    }

    public interface IBlobStorageService
    {
        string Name => GetType().Name;

        bool SupportsSecurityTokens { get; }

        Uri RootUri { get; }

        ValueTask<bool> ExistsAsync(string blobPathName);

        ValueTask<IBlob> CreateAsync(string filename, string path, Stream stream, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null);

        ValueTask<IBlob> CreateAsync(string filename, string path, byte[] data, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null);

        ValueTask<IBlob> GetBlobAsync(string blobPathName);

        ValueTask<Stream> OpenReadAsync(string blobPathName);

        IAsyncEnumerable<IBlob> GetBlobsAsync(string pathName);

        ValueTask DeleteAsync(string blobPathName);

        ValueTask<IBlob> CopyAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false);

        ValueTask<IBlob> MoveAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false);

        ValueTask<IBlob> RenameAsync(string sourceBlobPathName, string newBlobName);

        ValueTask<string> GenerateSecurityTokenAsync(string container, string? blobPathName = null, int durationMinutes = 60, BlobStorageSecurityTarget target = BlobStorageSecurityTarget.Container, BlobStorageSecurityPermissions permissions = BlobStorageSecurityPermissions.Read, int cacheMinutes = 60);
    }
}
