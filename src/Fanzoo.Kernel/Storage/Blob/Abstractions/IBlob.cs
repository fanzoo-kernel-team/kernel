namespace Fanzoo.Kernel.Storage.Blob
{
    public interface IBlob
    {
        Guid Id { get; }

        string Filename { get; }

        string Path { get; }

        long Size { get; }

        string MediaType { get; }

        IReadOnlyDictionary<string, string> Metadata { get; }

        bool IsReadOnly { get; }

        DateTimeOffset? Created { get; }

        DateTimeOffset? LastModified { get; }

        DateTimeOffset? LastAccessed { get; }
    }
}
