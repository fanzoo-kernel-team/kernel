using Microsoft.Extensions.Options;
using static System.IO.Path;

namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class FileBlobStorageSettings
    {
        public const string SectionName = $"{BlobStorageServiceFactorySettings.SectionName}:Settings";

        public string RootPath { get; set; } = default!;
    }

    public sealed class FileBlob : IBlob
    {
        public FileBlob(FileInfo fileInfo)
        {
            var fileName = GetFileNameWithoutExtension(fileInfo.FullName);

            if (Guid.TryParse(fileName, out var id) is false)
            {
                throw new ArgumentException($"File name {fileName} is not a valid Guid", nameof(fileInfo));
            }

            Id = id;
            Filename = id.ToString();
            Path = GetDirectoryName(fileInfo.FullName)!; //why would this be null?
            Size = fileInfo.Length;
            MediaType = "application/octet-stream";
            Metadata = new Dictionary<string, string>();
            IsReadOnly = fileInfo.IsReadOnly;
            Created = fileInfo.CreationTimeUtc;
            LastModified = fileInfo.LastWriteTimeUtc;
            LastAccessed = fileInfo.LastAccessTimeUtc;
        }

        public Guid Id { get; }

        public string Filename { get; }

        public string Path { get; }

        public long Size { get; }

        public string MediaType { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public bool IsReadOnly { get; }

        public DateTimeOffset? Created { get; }

        public DateTimeOffset? LastModified { get; }

        public DateTimeOffset? LastAccessed { get; }
    }

    public sealed class FileBlobStorageService : IBlobStorageService
    {
        private readonly FileBlobStorageSettings _settings;

        public FileBlobStorageService(IOptions<FileBlobStorageSettings> options)
        {
            _settings = options.Value;
        }

        public string Name => "File";

        public async ValueTask<IBlob> CreateAsync(Guid id, string filename, string path, Stream stream, string mediaType, bool isReadOnly = false, bool overwrite = false)
        {
            filename = id.ToString();

            var filePathname = Combine(_settings.RootPath, path, filename);

            if (overwrite is false && File.Exists(filePathname))
            {
                throw new ArgumentException($"File {filePathname} already exists", nameof(id));
            }

            stream.Position = 0;

            if (Directory.Exists(GetDirectoryName(filePathname)) is false)
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

        public ValueTask<IBlob> CreateAsync(Guid id, string filename, string path, byte[] data, string mediaType, bool isReadOnly = false, bool overwrite = false) => CreateAsync(id, filename, path, new MemoryStream(data), mediaType, isReadOnly, overwrite);

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

            if (File.Exists(filePathname) is false)
            {
                throw new ArgumentException($"File {filePathname} does not exist", nameof(blobPathName));
            }

            File.Delete(filePathname);

            return default;
        }

        public ValueTask<IBlob> CopyAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var sourceFilePathname = Combine(_settings.RootPath, sourceBlobPathName);

            if (File.Exists(sourceFilePathname) is false)
            {
                throw new ArgumentException($"File {sourceFilePathname} does not exist", nameof(sourceBlobPathName));
            }

            var destinationFilePathname = Combine(_settings.RootPath, destinationBlobPathName);

            if (overwrite is false && File.Exists(destinationFilePathname))
            {
                throw new ArgumentException($"File {destinationFilePathname} already exists", nameof(destinationBlobPathName));
            }

            File.Copy(sourceFilePathname, destinationFilePathname, overwrite);

            return new ValueTask<IBlob>(new FileBlob(new FileInfo(destinationFilePathname)));
        }

        public ValueTask<IBlob> MoveAsync(string sourceBlobPathName, string destinationBlobPathName, bool overwrite = false)
        {
            var sourceFilePathname = Combine(_settings.RootPath, sourceBlobPathName);

            if (File.Exists(sourceFilePathname) is false)
            {
                throw new ArgumentException($"File {sourceFilePathname} does not exist", nameof(sourceBlobPathName));
            }

            var destinationFilePathname = Combine(_settings.RootPath, destinationBlobPathName);

            if (overwrite is false && File.Exists(destinationFilePathname))
            {
                throw new ArgumentException($"File {destinationFilePathname} already exists", nameof(destinationBlobPathName));
            }

            File.Move(sourceFilePathname, destinationFilePathname, overwrite);

            return new ValueTask<IBlob>(new FileBlob(new FileInfo(destinationFilePathname)));
        }
    }
}
