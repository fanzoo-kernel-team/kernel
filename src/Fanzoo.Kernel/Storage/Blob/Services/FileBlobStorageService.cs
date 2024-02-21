using Microsoft.Extensions.Options;
using static System.IO.Path;

namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class FileBlobStorageSettings
    {
        public const string SectionName = $"{BlobStorageServiceFactorySettings.SectionName}:Settings";

        public string RootPath { get; set; } = default!;
    }

    public sealed class FileBlob(FileInfo fileInfo) : IBlob
    {
        public Guid Id { get; }

        public string Filename { get; } = GetFileName(fileInfo.FullName);

        public string Path { get; } = GetDirectoryName(fileInfo.FullName)!; //why would this be null?

        public long Size { get; } = fileInfo.Length;

        public string MediaType { get; } = "application/octet-stream";

        public IReadOnlyDictionary<string, string> Metadata { get; } = new Dictionary<string, string>();

        public bool IsReadOnly { get; } = fileInfo.IsReadOnly;

        public DateTimeOffset? Created { get; } = fileInfo.CreationTimeUtc;

        public DateTimeOffset? LastModified { get; } = fileInfo.LastWriteTimeUtc;

        public DateTimeOffset? LastAccessed { get; } = fileInfo.LastAccessTimeUtc;
    }

    public sealed class FileBlobStorageService(IOptions<FileBlobStorageSettings> options) : IBlobStorageService
    {
        private readonly FileBlobStorageSettings _settings = options.Value;

        public string Name => "File";

        public bool SupportsSecurityTokens => false;

        public Uri RootUri => new($"{_settings.RootPath}\\");

        public ValueTask<bool> ExistsAsync(string blobPathName)
        {
            var filePathname = Combine(_settings.RootPath, blobPathName);

            return new ValueTask<bool>(File.Exists(filePathname));
        }

        public async ValueTask<IBlob> CreateAsync(string filename, string path, Stream stream, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null)
        {
            var filePathname = Combine(_settings.RootPath, path, filename);

            if (!overwrite && File.Exists(filePathname))
            {
                throw new ArgumentException($"File {filePathname} already exists", nameof(filename));
            }

            stream.Position = 0;

            if (!Directory.Exists(GetDirectoryName(filePathname)))
            {
                Directory.CreateDirectory(GetDirectoryName(filePathname)!);
            }

            using var fileStream = new FileStream(filePathname, FileMode.Create, FileAccess.Write);

            await stream.CopyToAsync(fileStream);

            if (isReadOnly)
            {
                File.SetAttributes(filePathname, FileAttributes.ReadOnly);
            }

            return new FileBlob(new FileInfo(filePathname));
        }

        public ValueTask<IBlob> CreateAsync(string filename, string path, byte[] data, string mediaType, bool isReadOnly = false, bool overwrite = false, string? originalFilename = null) => CreateAsync(filename, path, new MemoryStream(data), mediaType, isReadOnly, overwrite, originalFilename);

        public ValueTask<IBlob> GetBlobAsync(string blobPathName)
        {
            var filePathname = Combine(_settings.RootPath, blobPathName);

            return File.Exists(filePathname) is false
                ? throw new ArgumentException($"File {filePathname} does not exist", nameof(blobPathName))
                : new ValueTask<IBlob>(new FileBlob(new FileInfo(filePathname)));
        }

        public ValueTask<Stream> OpenReadAsync(string blobPathName)
        {
            var filePathname = Combine(_settings.RootPath, blobPathName);

            return File.Exists(filePathname) is false
                ? throw new ArgumentException($"File {filePathname} does not exist", nameof(blobPathName))
                : new ValueTask<Stream>(new FileStream(filePathname, FileMode.Open, FileAccess.Read));
        }

        public ValueTask DeleteAsync(string blobPathName)
        {
            var filePathname = Combine(_settings.RootPath, blobPathName);

            if (!File.Exists(filePathname))
            {
                throw new ArgumentException($"File {filePathname} does not exist", nameof(blobPathName));
            }

            File.Delete(filePathname);

            return default;
        }

        public ValueTask DeleteContainerAsync(string container) => throw new NotImplementedException();


        public async IAsyncEnumerable<IBlob> GetBlobsAsync(string pathName)
        {
            pathName = Combine(_settings.RootPath, pathName);

            foreach (var file in await Task.Run(() => Directory.EnumerateFiles(pathName)))
            {
                yield return new FileBlob(new FileInfo(file));
            }
        }

        public ValueTask<IBlob> CopyAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var sourceFilePathname = Combine(_settings.RootPath, sourceBlobPathName);

            if (!File.Exists(sourceFilePathname))
            {
                throw new ArgumentException($"File {sourceFilePathname} does not exist", nameof(sourceBlobPathName));
            }

            var destinationFilePathname = Combine(_settings.RootPath, destinationBlobPathName);

            if (!overwrite && File.Exists(destinationFilePathname))
            {
                throw new ArgumentException($"File {destinationFilePathname} already exists", nameof(destinationBlobPathName));
            }

            File.Copy(sourceFilePathname, destinationFilePathname, overwrite);

            return new ValueTask<IBlob>(new FileBlob(new FileInfo(destinationFilePathname)));
        }

        public ValueTask<IBlob> MoveAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var sourceFilePathname = Combine(_settings.RootPath, sourceBlobPathName);

            if (!File.Exists(sourceFilePathname))
            {
                throw new ArgumentException($"File {sourceFilePathname} does not exist", nameof(sourceBlobPathName));
            }

            var destinationFilePathname = Combine(_settings.RootPath, destinationBlobPathName);

            if (!overwrite && File.Exists(destinationFilePathname))
            {
                throw new ArgumentException($"File {destinationFilePathname} already exists", nameof(destinationBlobPathName));
            }

            File.Move(sourceFilePathname, destinationFilePathname, overwrite);

            return new ValueTask<IBlob>(new FileBlob(new FileInfo(destinationFilePathname)));
        }

        public ValueTask<IBlob> RenameAsync(string sourceBlobPathName, string newBlobName)
        {
            var sourceFilePathName = Combine(_settings.RootPath, sourceBlobPathName);

            var destinationFilePathName = Combine(_settings.RootPath, GetDirectoryName(sourceBlobPathName)!, newBlobName);

            File.Move(sourceFilePathName, destinationFilePathName);

            return new ValueTask<IBlob>(new FileBlob(new FileInfo(destinationFilePathName)));
        }

        public ValueTask<string> GenerateSecurityTokenAsync(string container, string? blobPathName = null, int durationMinutes = 60, BlobStorageSecurityTarget target = BlobStorageSecurityTarget.Container, BlobStorageSecurityPermissions permissions = BlobStorageSecurityPermissions.Read, int cacheMinutes = 60) => throw new NotImplementedException();
    }
}
