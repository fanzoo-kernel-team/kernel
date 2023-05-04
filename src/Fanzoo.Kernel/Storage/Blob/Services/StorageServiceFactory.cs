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

    public sealed class BlobStorageServiceFactory : IBlobStorageServiceFactory
    {
        private readonly string _serviceName;

        private readonly IEnumerable<IBlobStorageService> _services;
        public BlobStorageServiceFactory(IOptions<BlobStorageServiceFactorySettings> settings, IEnumerable<IBlobStorageService> services)
        {
            _services = services;
            _serviceName = settings.Value.Service;
        }

        public IBlobStorageService GetService()
        {
            var service = _services.SingleOrDefault(s => s.Name == _serviceName);

            return service ?? throw new InvalidOperationException($"No blob storage service found with name {_serviceName}");
        }
    }
}
