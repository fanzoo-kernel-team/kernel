using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class AzureBlobStorageSettings
    {
        public const string SectionName = $"{BlobStorageServiceFactorySettings.SectionName}:Settings";

        public string ConnectionString { get; set; } = default!;
    }

    public sealed class AzureBlob : IBlob
    {
        public const string FileNameMetadataKey = "original_filename";

        public AzureBlob(string filename, string path, BlobProperties? properties = null)
        {
            Filename = filename;
            Path = path;
            Size = properties?.ContentLength ?? 0;
            MediaType = properties?.ContentType ?? string.Empty;
            Metadata = (IReadOnlyDictionary<string, string>)(properties?.Metadata ?? new Dictionary<string, string>());
            Created = properties?.CreatedOn;
            LastModified = properties?.LastModified;
            LastAccessed = properties?.LastAccessed;
        }

        public Guid Id { get; }

        public string Filename { get; }

        public string Path { get; }

        public long Size { get; }

        public string MediaType { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public bool IsReadOnly { get; } = true;

        public DateTimeOffset? Created { get; }

        public DateTimeOffset? LastModified { get; }

        public DateTimeOffset? LastAccessed { get; }
    }

    public sealed class AzureBlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;

        private Uri? _rootUri;

        public AzureBlobStorageService(IOptions<AzureBlobStorageSettings> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public string Name => "Azure";

        public bool SupportsSecurityTokens => true;

        public Uri RootUri => _rootUri ??= new BlobServiceClient(_connectionString).Uri;

        public async ValueTask<bool> ExistsAsync(string blobPathName)
        {
            var blobClient = GetBlobClient(blobPathName);

            return await blobClient.ExistsAsync();
        }

        public async ValueTask<IBlob> CreateAsync(string filename, string path, Stream stream, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null)
        {
            var (container, folder) = GetContainerAndFolderPath(path);

            var blobName = string.Join('\\', folder, filename).Trim('\\');

            var containerClient = new BlobContainerClient(_connectionString, container);

            //if the container doesn't exist, create it
            _ = await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);

            //if overwrite = false, then check if it exists first and throw an exception
            _ = !overwrite && await blobClient.ExistsAsync()
                ? throw new InvalidOperationException($"Blob {blobName} already exists.")
                : await blobClient.DeleteIfExistsAsync();

            var options = new BlobUploadOptions()
            {
                HttpHeaders = new()
                {
                    ContentType = mediaType
                }
            };

            var result = await blobClient.UploadAsync(stream, options);

            var uploadBlobResponse = result.GetRawResponse();

            if (uploadBlobResponse.IsError)
            {
                throw new InvalidOperationException($"Failed to create blob. Reason: {uploadBlobResponse.ReasonPhrase}");
            }

            //add metadata
            if (originalFilename is not null)
            {
                var metadata = new Dictionary<string, string>
                {
                    { AzureBlob.FileNameMetadataKey, originalFilename }
                };
                _ = await blobClient.SetMetadataAsync(metadata);
            }

            var getPropertiesResponse = await blobClient.GetPropertiesAsync();

            //we're going to be optimistic here and assume this succeeds... for now
            var properties = getPropertiesResponse.Value;

            return new AzureBlob(filename, path, properties);
        }

        public async ValueTask<IBlob> CreateAsync(string filename, string path, byte[] data, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null)
        {
            using var stream = new MemoryStream(data);

            return await CreateAsync(filename, path, stream, mediaType, isReadOnly, overwrite, originalFilename);
        }

        public async ValueTask<IBlob> GetBlobAsync(string blobPathName)
        {
            var (container, path, filename) = GetContainerAndBlobFolderPath(blobPathName);

            var blobClient = GetBlobClient(blobPathName);

            var getPropertiesResponse = await blobClient.GetPropertiesAsync();

            //we're going to be optimistic here and assume this succeeds... for now
            var properties = getPropertiesResponse.Value;

            return new AzureBlob(filename, string.Join('\\', container, path), properties);
        }

        public async ValueTask<Stream> OpenReadAsync(string blobPathName) => await GetBlobClient(blobPathName).OpenReadAsync();

        public async ValueTask DeleteAsync(string blobPathName) => await GetBlobClient(blobPathName).DeleteIfExistsAsync();

        public async ValueTask<IBlob> CopyAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var sourceBlobClient = GetBlobClient(sourceBlobPathName);

            var lease = sourceBlobClient.GetBlobLeaseClient();

            var leaseResult = await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

            if (leaseResult.Value is not null)
            {
                try
                {
                    var destinationBlobClient = GetBlobClient(destinationBlobPathName);

                    var exists = await destinationBlobClient.ExistsAsync();

                    if (exists && overwrite)
                    {
                        await DeleteAsync(destinationBlobPathName);
                    }
                    else if (exists && !overwrite)
                    {
                        throw new InvalidOperationException($"Destination blob already exists.");
                    }

                    var copyResult = await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);

                    _ = await copyResult.WaitForCompletionAsync();
                }
                finally
                {
                    _ = await lease.BreakAsync();
                }
            }

            return await GetBlobAsync(destinationBlobPathName);
        }

        public async ValueTask<IBlob> MoveAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var blob = await CopyAsync(sourceBlobPathName, destinationBlobPathName, overwrite);

            await DeleteAsync(sourceBlobPathName);

            return blob;
        }

        public async ValueTask<IBlob> RenameAsync(string sourceBlobPathName, string newBlobName)
        {
            var (container, path, _) = GetContainerAndBlobFolderPath(sourceBlobPathName);

            var destinationBlobPathName = string.Join('\\', container, path, newBlobName).Trim('\\');

            return await MoveAsync(sourceBlobPathName, destinationBlobPathName, false);
        }


        public async ValueTask DeleteContainerAsync(string container)
        {
            var containerClient = GetContainerClient(container);

            try
            {
                // Delete the specified container and handle the exception.
                await containerClient.DeleteIfExistsAsync();
            }
            catch
            {
                throw new InvalidOperationException($"Failed to delete container");
            }
        }

        public ValueTask<string> GenerateSecurityTokenAsync(string container, string? blobPathName = null, int durationMinutes = 60, BlobStorageSecurityTarget target = BlobStorageSecurityTarget.Container, BlobStorageSecurityPermissions permissions = BlobStorageSecurityPermissions.Read, int cacheMinutes = 60)
        {
            //clean up the container name (this is critical, because it will not work if the container name starts with a slash)
            container = container.TrimStart('/').TrimStart('\\');

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container,
                BlobName = blobPathName ?? string.Empty,
                Resource = target.ToAzureSasResource(),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(durationMinutes)
            };

            if (target == BlobStorageSecurityTarget.Blob && cacheMinutes > 0)
            {
                sasBuilder.CacheControl = $"public, max-age={cacheMinutes * 60}";
            }

            sasBuilder.SetPermissions(permissions.ToAzureSasPermissions());

            var containerClient = new BlobContainerClient(_connectionString, container);

            return containerClient.CanGenerateSasUri is false
                ? throw new InvalidOperationException($"Container {container} does not support generating SAS tokens.")
                : new(containerClient.GenerateSasUri(sasBuilder).Query.TrimStart('?'));
        }

        public async IAsyncEnumerable<IBlob> GetBlobsAsync(string pathName)
        {
            var (container, path) = GetContainerAndFolderPath(pathName);

            var containerClient = new BlobContainerClient(_connectionString, container);

            await foreach (var blob in containerClient.GetBlobsAsync(prefix: path))
            {
                yield return new AzureBlob(blob.Name, path, null);
            }
        }

        private static (string Container, string Path) GetContainerAndFolderPath(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            //clean it up
            path = path.Trim();

            //normalize the slashes
            path = path.Replace("/", "\\");

            var pathElements = path.Split('\\');

            //the first element will always be the container
            var container = pathElements[0].Trim().ToLower();

            //everything after it will be the path
            path = string.Join('\\', pathElements[1..]).TrimEnd('\\');

            return (container, path);

        }

        private static (string Container, string Path, string Filename) GetContainerAndBlobFolderPath(string blobPathName)
        {
            var (container, path) = GetContainerAndFolderPath(blobPathName);

            var pathSegments = path.Split('\\');

            var filename = pathSegments[^1];

            path = string.Join('\\', pathSegments[..^1]).TrimEnd('\\');

            return (container, path, filename);
        }

        private BlobClient GetBlobClient(string blobPathName)
        {
            var (container, path, filename) = GetContainerAndBlobFolderPath(blobPathName);

            var blobName = string.Join('\\', path, filename).TrimStart('\\').TrimEnd('\\');

            var containerClient = new BlobContainerClient(_connectionString, container);

            return containerClient.GetBlobClient(blobName);
        }

        private BlobContainerClient GetContainerClient(string container)
        {
            var containerClient = new BlobContainerClient(_connectionString, container);

            return containerClient;
        }
    }

    static file class Extensions
    {
        public static string ToAzureSasResource(this BlobStorageSecurityTarget target) => target switch
        {
            BlobStorageSecurityTarget.Blob => "b",
            BlobStorageSecurityTarget.Container => "c",
            _ => throw new NotSupportedException($"Security target {target} is not supported."),
        };

        public static BlobSasPermissions ToAzureSasPermissions(this BlobStorageSecurityPermissions permission) => permission switch
        {
            BlobStorageSecurityPermissions.Read => BlobSasPermissions.Read,
            BlobStorageSecurityPermissions.Add => BlobSasPermissions.Add,
            BlobStorageSecurityPermissions.Create => BlobSasPermissions.Create,
            BlobStorageSecurityPermissions.Write => BlobSasPermissions.Write,
            BlobStorageSecurityPermissions.Delete => BlobSasPermissions.Delete,
            BlobStorageSecurityPermissions.Tag => BlobSasPermissions.Tag,
            BlobStorageSecurityPermissions.DeleteBlobVersion => BlobSasPermissions.DeleteBlobVersion,
            BlobStorageSecurityPermissions.List => BlobSasPermissions.List,
            BlobStorageSecurityPermissions.Move => BlobSasPermissions.Move,
            BlobStorageSecurityPermissions.Execute => BlobSasPermissions.Execute,
            BlobStorageSecurityPermissions.SetImmutabilityPolicy => BlobSasPermissions.SetImmutabilityPolicy,
            BlobStorageSecurityPermissions.PermanentDelete => BlobSasPermissions.PermanentDelete,
            BlobStorageSecurityPermissions.All => BlobSasPermissions.All,
            _ => throw new NotSupportedException($"Security permission {permission} is not supported."),
        };
    }
}
