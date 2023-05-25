namespace Fanzoo.Kernel.Web.Services
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }

        string GetClaim(string claim);

        string? GetClaimOrDefault(string claim);

        bool TryGetClaim(string claim, out string value);

        IEnumerable<Claim> GetClaims(string claim);
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

        public IEnumerable<Claim> GetClaims(string claim) => _contextAccessor.User?.GetClaims(claim) ?? Enumerable.Empty<Claim>();
    }

    public static class CurrentUserServiceExtensions
    {
        public static string? GetUserIdOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.PrimarySid);

        public static string GetUserId(this ICurrentUserService service) => service.GetClaim(System.Security.Claims.ClaimTypes.PrimarySid);

        public static T GetUserId<T>(this ICurrentUserService service) => typeof(T) switch
        {
            var t when t == typeof(Guid) => (T)Convert.ChangeType(new Guid(service.GetUserId()), typeof(T)),
            var t when t == typeof(int) => (T)Convert.ChangeType(int.Parse(service.GetUserId()), typeof(T)),
            var t when t == typeof(string) => (T)Convert.ChangeType(service.GetUserId(), typeof(T)),
            _ => throw new InvalidOperationException("Unsupported type.")
        };

        public static string? GetUsernameOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(ClaimTypes.Username);

        public static string GetUsername(this ICurrentUserService service) => service.GetClaim(ClaimTypes.Username);

        public static string? GetEmailOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.Email);

        public static string GetEmail(this ICurrentUserService service) => service.GetClaim(System.Security.Claims.ClaimTypes.Email);

        public static string? GetNameOrDefault(this ICurrentUserService service) => service.GetClaimOrDefault(System.Security.Claims.ClaimTypes.Name);

        public static string GetName(this ICurrentUserService service) => service.GetClaim(System.Security.Claims.ClaimTypes.Name);

        public static IEnumerable<string> GetRoles(this ICurrentUserService service) => service.GetClaims(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);

        public static bool IsInRole(this ICurrentUserService service, string role) => service.GetRoles().Contains(role);

    }
}
