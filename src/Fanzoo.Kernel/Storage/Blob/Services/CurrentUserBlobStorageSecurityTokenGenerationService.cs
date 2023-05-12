using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class CurrentUserBlobStorageSecurityTokenGenerationServiceSettings
    {
        public const string SectionName = $"{BlobStorageServiceFactorySettings.SectionName}:Settings";

        public int SecurityTokenDurationMinutes { get; set; } = 60;
    }

    public class CurrentUserBlobStorageSecurityTokenGenerationService : IBlobStorageSecurityTokenGenerationService
    {
        private readonly IBlobStorageServiceFactory _blobStorageServiceFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly CurrentUserBlobStorageSecurityTokenGenerationServiceSettings _settings;

        public CurrentUserBlobStorageSecurityTokenGenerationService(IOptions<CurrentUserBlobStorageSecurityTokenGenerationServiceSettings> options, IBlobStorageServiceFactory blobStorageServiceFactory, ICurrentUserService currentUserService)
        {
            _blobStorageServiceFactory = blobStorageServiceFactory;
            _currentUserService = currentUserService;
            _settings = options.Value;
        }

        public int SecurityTokenDurationMinutes => _settings.SecurityTokenDurationMinutes;

        public ValueTask<string> GenerateContainerSecurityTokenAsync()
        {
            var path = $"{_currentUserService.GetUserId()}";

            var service = _blobStorageServiceFactory.GetService();

            return service.GenerateSecurityTokenAsync(path, durationMinutes: _settings.SecurityTokenDurationMinutes);
        }

        public ValueTask<string> GenerateBlobSecurityTokenAsync(string blobPathName) => throw new NotImplementedException();
    }
}
