using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class BlobStorageServiceFactorySettings
    {
        public const string SectionName = "BlobStorage";

        public string Service { get; set; } = default!;
    }

    public interface IBlobStorageServiceFactory
    {
        IBlobStorageService GetService();
    }

    public sealed class BlobStorageServiceFactory(IOptions<BlobStorageServiceFactorySettings> settings, IEnumerable<IBlobStorageService> services) : IBlobStorageServiceFactory
    {
        private readonly string _serviceName = settings.Value.Service;

        private readonly IEnumerable<IBlobStorageService> _services = services;

        public IBlobStorageService GetService()
        {
            var service = _services.SingleOrDefault(s => s.Name == _serviceName);

            return service ?? throw new InvalidOperationException($"No blob storage service found with name {_serviceName}");
        }
    }
}
