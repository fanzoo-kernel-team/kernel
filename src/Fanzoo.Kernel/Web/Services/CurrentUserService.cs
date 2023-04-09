namespace Fanzoo.Kernel.Web.Services
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }

        string GetClaim(string claim);

        string? GetClaimOrDefault(string claim);

        bool TryGetClaim(string claim, out string value);
    }

    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IContextAccessorService _contextAccessor;

        public CurrentUserService(IContextAccessorService contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated => _contextAccessor.User?.Identity?.IsAuthenticated ?? false;

        public string? GetClaimOrDefault(string claim) => _contextAccessor.User?.GetClaimOrDefault(claim)?.Value;

        public string GetClaim(string claim) => _contextAccessor.User?.GetClaimOrDefault(claim)?.Value
                ?? throw new InvalidOperationException("Claim not found.");

        public bool TryGetClaim(string claim, out string value)
        {
            var result = _contextAccessor.User?.GetClaimOrDefault(claim)?.Value;

            if (result is not null)
            {
                value = result;
                return true;
            }
            else
            {
                value = null!;
                return false;
            }
        }
    }

    public static class CurrentUserServiceExtensions
    {
        public static string? GetUserIdOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.PrimarySid);

        public static string? GetUsernameOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(ClaimTypes.Username);

        public static string? GetEmailOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.Email);

        public static string? GetNameOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.Name);
    }
}
