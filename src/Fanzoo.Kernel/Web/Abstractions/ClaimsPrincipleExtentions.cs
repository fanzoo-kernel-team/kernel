namespace System.Security.Claims
{
    public static class ClaimsPrincipleExtentions
    {
        public static Claim? GetClaimOrDefault(this ClaimsPrincipal user, string claimType) =>
            user.Identity is not null
                && user.Identity.IsAuthenticated
                && user.HasClaim(c => c.Type == claimType)
                    ? user.Claims.Single(c => c.Type == claimType)
                    : null;

        public static IEnumerable<Claim> GetClaims(this ClaimsPrincipal user, string claimType) =>
            user.Identity is not null
                && user.Identity.IsAuthenticated
                && user.HasClaim(c => c.Type == claimType)
                    ? user.Claims.Where(c => c.Type == claimType)
                    : [];
    }
}
