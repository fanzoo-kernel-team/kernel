#pragma warning disable S101 // Types should be named in PascalCase

using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fanzoo.Kernel.Web.Services
{
    public abstract class RESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive> :
        IRESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUser : IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive, TIdentifier, TPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
    {
        private readonly JwtSecurityTokenSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHashingService _passwordHashingService;

        protected RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService)
        {
            _settings = settings.Value;
            _httpContextAccessor = httpContextAccessor;
            _passwordHashingService = passwordHashingService;
        }

        public async ValueTask<Result<(string AccessToken, string RefreshToken), Error>> AuthenticateAsync(TUsername username, TPassword password)
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

            var (accessToken, refreshToken) = await GetTokensAsync(user);

            await SaveUserAsync();

            return (new JwtSecurityTokenHandler().WriteToken(accessToken).ToString(), refreshToken.Token);
        }

        public async ValueTask<Result<(string AccessToken, string RefreshToken), Error>> RefreshTokenAsync(string refreshToken)
        {
            var user = await GetUserByTokenAsync(RefreshTokenValue.Create(refreshToken).Value);

            if (user is null)
            {
                return Errors.UserAuthentication.InvalidRefreshToken;
            }

            if (!user.IsActive)
            {
                return Errors.UserAuthentication.AccountIsNotActive;
            }

            if (user.IsLockedOut)
            {
                return Errors.UserAuthentication.AccountIsLocked;
            }

            var token = user.RefreshTokens.SingleOrDefault(t => t.Token == RefreshTokenValue.Create(refreshToken).Value);

            if (token is null)
            {
                return Errors.UserAuthentication.InvalidRefreshToken;
            }

            if (token.IsRevoked)
            {
                return Errors.UserAuthentication.TokenIsRevoked;
            }

            if (token.IsRevoked)
            {
                return Errors.UserAuthentication.TokenIsExpired;
            }

            var (accessToken, newRefreshToken) = await GetTokensAsync(user);

            await SaveUserAsync();

            return (new JwtSecurityTokenHandler().WriteToken(accessToken).ToString(), newRefreshToken.Token);
        }

        public async ValueTask<UnitResult<Error>> RevokeAsync(string refreshToken)
        {
            var token = RefreshTokenValue.Create(refreshToken).Value;

            var user = await GetUserByTokenAsync(token);

            if (user is null)
            {
                return Errors.UserAuthentication.InvalidRefreshToken;
            }

            user.RevokeToken(token);

            await SaveUserAsync();

            return UnitResult.Success<Error>();
        }

        public async ValueTask<UnitResult<Error>> RevokeAllAsync(TIdentifier identifier)
        {
            var user = await GetUserByIdAsync(identifier);

            if (user is null)
            {
                return Errors.UserAuthentication.UserNotFound;
            }

            user.RevokeAllTokens();

            await SaveUserAsync();

            return UnitResult.Success<Error>();
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

        protected abstract ValueTask<IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>?> GetUserByUsernameAsync(TUsername username);

        protected abstract ValueTask<IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>?> GetUserByIdAsync(TIdentifier identifier);

        protected abstract ValueTask<IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>?> GetUserByTokenAsync(RefreshTokenValue token);

        protected abstract IAsyncEnumerable<Claim> GetClaimsAsync(IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> user);

        protected abstract ValueTask SaveUserAsync();

        private async ValueTask<JwtSecurityToken> GenerateAccessTokenAsync(IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> user)
        {
            var claims = new List<Claim>();

            claims
                .AddClaim(System.Security.Claims.ClaimTypes.PrimarySid, user.Id.Value)
                .AddClaim(ClaimTypes.Username, user.Username.Value)
                .AddClaim(System.Security.Claims.ClaimTypes.Email, user.Email)
                .AddClaim(JwtRegisteredClaimNames.Sub, user.Username.Value) // subject (required)
                .AddClaim(JwtRegisteredClaimNames.Jti, user.Id.Value.ToString()!) // token id is scoped to the user id
                .AddClaim(ClaimTypes.LastAuthenticationChange, user.LastAuthenticationChange.AddSeconds(1)); // there is some slight precision loss in the string round-trip, so we give it an extra second

            //add application claims
            await foreach (var claim in GetClaimsAsync(user))
            {
                claims.Add(claim);
            }

            return new(
                _settings.Issuer,
                _settings.Audience,
                claims,
                expires: SystemDateTime.Now.AddMinutes(_settings.AccessTokenTTLMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(_settings.Secret)), SecurityAlgorithms.HmacSha256));
        }

        private async ValueTask<(JwtSecurityToken AccessToken, IRefreshToken<TTokenIdentifier, TTokenPrimitive, TIdentifier, TPrimitive> RefreshToken)> GetTokensAsync(IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);

            var refreshToken = user
                .AddRefreshToken(
                    SystemDateTime.Now.AddMinutes(_settings.RefreshTokenTTLMinutes),
                    IPAddressValue.Create(_httpContextAccessor.GetIPv4Address()).Value);

            return (accessToken, refreshToken);
        }
    }
}
