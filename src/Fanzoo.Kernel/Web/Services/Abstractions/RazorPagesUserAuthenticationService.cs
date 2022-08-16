using Fanzoo.Kernel.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.Web.Services
{
    public abstract class RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword> : IRazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
        where TUser : IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TPassword : IPasswordValue
    {
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService)
        {
            _passwordHashingService = passwordHashingService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<UnitResult<Error>> SignInAsync(TUsername username, TPassword password)
        {
            var user = await GetUserByUsernameAsync(username);

            if (user is null)
            {
                return Errors.UserAuthentication.UserNotFound;
            }

            if (!user.IsActive)
            {
                return Errors.UserAuthentication.AccountIsNotActive;
            }

            if (user.IsLockedOut)
            {
                return Errors.UserAuthentication.AccountIsLocked;
            }

            if (!_passwordHashingService.VerifyPasswordHash(user.Username.Value, user.Password.Value, password.Value))
            {
                user.RecordInvalidLogin();

                await SaveUserAsync();

                return Errors.UserAuthentication.PasswordVerificationFailed;
            }

            user.RecordValidLogin();

            var claims = new List<Claim>();

            claims
                .AddClaim(System.Security.Claims.ClaimTypes.PrimarySid, user.Id.Value)
                .AddClaim(ClaimTypes.Username, user.Username.Value)
                .AddClaim(System.Security.Claims.ClaimTypes.Email, user.Email)
                .AddClaim(ClaimTypes.LastAuthenticationChange, user.LastAuthenticationChange.AddSeconds(1)); // there is some slight precision loss in the string round-trip, so we give it an extra second

            //add application claims
            await foreach (var claim in GetClaimsAsync(user))
            {
                claims.Add(claim);
            }

            await _httpContextAccessor
                .HttpContext!
                    .SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                        new() { IsPersistent = true });

            await SaveUserAsync();

            return UnitResult.Success<Error>();
        }

        public async ValueTask SignOutAsync(TIdentifier identifier)
        {
            var user = await GetUserByIdAsync(identifier);

            if (user is null)
            {
                return;
            }

            user.SignOut();

            await _httpContextAccessor
                .HttpContext!
                    .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await SaveUserAsync();
        }

        public async Task ValidateLastAuthenticationChangeAsync(CookieValidatePrincipalContext context)
        {
            if (context.Principal is null || await GetRequiresAuthenticationAsync(context.Principal))
            {
                context.RejectPrincipal();
            }
        }

        protected async ValueTask<bool> GetRequiresAuthenticationAsync(ClaimsPrincipal principal)
        {
            var identifier = GetClaimIdentifier(principal.Claims.GetClaimValueOrDefault(System.Security.Claims.ClaimTypes.PrimarySid));

            if (identifier is not null)
            {
                var user = await GetUserByIdAsync(identifier);

                if (user is not null)
                {
                    var lastAuthenticationChange = principal.Claims.GetClaimValueOrDefault(ClaimTypes.LastAuthenticationChange);

                    if (lastAuthenticationChange is not null
                        && DateTime.TryParse(lastAuthenticationChange, out var lastAuthenticationChangeDate)
                        && !user.RequiresAuthentication(lastAuthenticationChangeDate))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected abstract TIdentifier? GetClaimIdentifier(string? claimValue);

        protected abstract ValueTask<IUser<TIdentifier, TPrimitive, TUsername>?> GetUserByUsernameAsync(TUsername username);

        protected abstract ValueTask<IUser<TIdentifier, TPrimitive, TUsername>?> GetUserByIdAsync(TIdentifier identifier);

        protected abstract IAsyncEnumerable<Claim> GetClaimsAsync(IUser<TIdentifier, TPrimitive, TUsername> user);

        protected abstract ValueTask SaveUserAsync();

    }
}
