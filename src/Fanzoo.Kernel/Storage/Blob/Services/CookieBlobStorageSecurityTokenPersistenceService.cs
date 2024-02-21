namespace Fanzoo.Kernel.Storage.Blob.Services
{
    public sealed class CookieBlobStorageSecurityTokenPersistenceService(IBlobStorageSecurityTokenGenerationService securityTokenGenerationService, IHttpContextAccessor httpContextAccessor) : IBlobStorageSecurityTokenPersistenceService
    {
        public const string SecurityTokenCookieName = "BlobStorageSecurityToken";
        public const string SecurityTokenExpirationCookieName = "BlobStorageSecurityTokenExpiration";

        private const int SkewMinutes = 5;

        private readonly IBlobStorageSecurityTokenGenerationService _securityTokenGenerationService = securityTokenGenerationService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async ValueTask<string> GetCurrentContainerSecurityTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No HttpContext found.");

            var generateToken = true;

            // check for existing token
            if (httpContext.Request.Cookies.TryGetValue(SecurityTokenCookieName, out var token) &&
                httpContext.Request.Cookies.TryGetValue(SecurityTokenExpirationCookieName, out var expirationString))
            {
                var expiration = DateTimeOffset.Parse(expirationString);

                generateToken = SystemDateTimeOffset.UtcNow >= expiration;
            }

            if (generateToken)
            {
                token = await _securityTokenGenerationService.GenerateContainerSecurityTokenAsync();

                var durationMinutes = _securityTokenGenerationService.SecurityTokenDurationMinutes;

                var expiration = SystemDateTimeOffset.UtcNow.AddMinutes(Math.Min(durationMinutes - SkewMinutes, durationMinutes));

                httpContext.Response.Cookies.Append(SecurityTokenCookieName, token, new CookieOptions { Secure = true, HttpOnly = true });
                httpContext.Response.Cookies.Append(SecurityTokenExpirationCookieName, expiration.ToString("o"), new CookieOptions { Secure = true, HttpOnly = true });
            }

            return token is not null ? token : throw new InvalidOperationException("Security token could not be generated.");
        }

        public async ValueTask ClearCurrentContainerSecurityTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is not null)
            {
                httpContext.Response.Cookies.Delete(SecurityTokenCookieName);
                httpContext.Response.Cookies.Delete(SecurityTokenExpirationCookieName);
            }

            await Task.CompletedTask;
        }
    }
}
