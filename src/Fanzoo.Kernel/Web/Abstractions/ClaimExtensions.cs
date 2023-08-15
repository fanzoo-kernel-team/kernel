namespace Fanzoo.Kernel.Web
{
    public static class ClaimExtensions
    {
        public static bool HasClaimType(this IEnumerable<Claim> claims, string claimType) => claims.Any(c => c.Type == claimType);

        public static string? GetClaimValueOrDefault(this IEnumerable<Claim> claims, string claimType) => claims.FirstOrDefault(c => c.Type == claimType)?.Value;

        public static IList<Claim> AddClaim(this IList<Claim> claims, string claimType, object value, bool supportsMultiple = false)
        {
            if (supportsMultiple || !claims.HasClaimType(claimType))
            {
                claims.Add(new Claim(claimType, value.ToString()!));
            }

            return claims;
        }
    }
}
