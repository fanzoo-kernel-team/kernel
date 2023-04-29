namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public interface IBlobStorageService
    {
        string Name => GetType().Name;

        ValueTask<IBlob> CreateAsync(Guid id, string filename, string path, Stream stream, string mediaType, bool isReadOnly = false, bool overwrite = false);

        ValueTask<IBlob> CreateAsync(Guid id, string filename, string path, byte[] data, string mediaType, bool isReadOnly = false, bool overwrite = false);

        ValueTask<IBlob> GetBlobAsync(string blobPathName);

        ValueTask<Stream> OpenReadAsync(string blobPathName);

        ValueTask DeleteAsync(string blobPathName);

        ValueTask<IBlob> CopyAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false);

        ValueTask<IBlob> MoveAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false);

        //TODO: Figure out how to "stream" OpenReadAsync efficiently
        //      To avoid having public access to storage

        //TODO: Change Access Tier
        //  For Containers (this is less critical)
        //  For Blobs
        //  For "Folders" (meaning, every blob with the folder prefix)

        //TODO: List Blobs or GetBlobsAsync(??)
        //  - given a container
        //  - given a container+folder (prefix)
    }
}
