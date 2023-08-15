#pragma warning disable S101 // Types should be named in PascalCase

using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fanzoo.Kernel.Web.Services
{
    public interface IRESTApiUserAuthenticationService<TIdentifier, TPrimitive, TUsername, TPassword>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TPassword : IPasswordValue
    {
        ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> AuthenticateAsync(TUsername username, TPassword password);

        ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> RefreshTokenAsync(string refreshToken);

        ValueTask<UnitResult<Error>> RevokeAsync(string refreshToken);

        ValueTask<UnitResult<Error>> RevokeAllAsync(TIdentifier identifier);

    }

    public abstract class RESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive> : IDisposable, IAsyncDisposable,
        IRESTApiUserAuthenticationService<TIdentifier, TPrimitive, TUsername, TPassword>
            where TUser : IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
    {
        private readonly JwtSecurityTokenSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private bool _disposed = false;

        protected RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _settings = settings.Value;
            _httpContextAccessor = httpContextAccessor;
            _passwordHashingService = passwordHashingService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> AuthenticateAsync(TUsername username, TPassword password)
        {
            _unitOfWorkFactory.Open();

            var user = await FindUserByUsernameAsync(username);

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

                _unitOfWorkFactory.Close();

                return Errors.UserAuthentication.PasswordVerificationFailed;
            }

            user.RecordValidLogin();

            var (accessToken, refreshToken) = await GetTokensAsync(user);

            await SaveUserAsync();

            _unitOfWorkFactory.Close();

            return (new JwtSecurityTokenHandler().WriteToken(accessToken).ToString(), refreshToken.Token);
        }

        public async ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> RefreshTokenAsync(string refreshToken)
        {
            _unitOfWorkFactory.Open();

            var user = await FindUserByTokenAsync(RefreshTokenValue.Create(refreshToken).Value);

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

            token.Revoke();

            await SaveUserAsync();

            _unitOfWorkFactory.Close();

            return (new JwtSecurityTokenHandler().WriteToken(accessToken).ToString(), newRefreshToken.Token);
        }

        public async ValueTask<UnitResult<Error>> RevokeAsync(string refreshToken)
        {
            _unitOfWorkFactory.Open();

            var token = RefreshTokenValue.Create(refreshToken).Value;

            var user = await FindUserByTokenAsync(token);

            if (user is null)
            {
                return Errors.UserAuthentication.InvalidRefreshToken;
            }

            user.RevokeToken(token);

            await SaveUserAsync();

            _unitOfWorkFactory.Close();

            return UnitResult.Success<Error>();
        }

        public async ValueTask<UnitResult<Error>> RevokeAllAsync(TIdentifier identifier)
        {
            _unitOfWorkFactory.Open();

            var user = await FindUserByIdAsync(identifier);

            if (user is null)
            {
                return Errors.UserAuthentication.UserNotFound;
            }

            user.RevokeAllTokens();

            await SaveUserAsync();

            _unitOfWorkFactory.Close();

            return UnitResult.Success<Error>();
        }

        protected async ValueTask<bool> GetRequiresAuthenticationAsync(ClaimsPrincipal principal)
        {
            _unitOfWorkFactory.Open();

            var identifier = FindClaimIdentifier(principal.Claims.GetClaimValueOrDefault(System.Security.Claims.ClaimTypes.PrimarySid));

            if (identifier is not null)
            {
                var user = await FindUserByIdAsync(identifier);

                if (user is not null)
                {
                    var lastAuthenticationChange = principal.Claims.GetClaimValueOrDefault(ClaimTypes.LastAuthenticationChange);

                    if (lastAuthenticationChange is not null
                        && DateTime.TryParse(lastAuthenticationChange, out var lastAuthenticationChangeDate)
                        && !user.RequiresAuthentication(lastAuthenticationChangeDate))
                    {
                        _unitOfWorkFactory.Close();

                        return false;
                    }
                }
            }

            _unitOfWorkFactory.Close();

            return true;
        }

        protected abstract TIdentifier? FindClaimIdentifier(string? claimValue);

        protected abstract ValueTask<TUser?> FindUserByUsernameAsync(TUsername username);

        protected abstract ValueTask<TUser?> FindUserByIdAsync(TIdentifier identifier);

        protected abstract ValueTask<TUser?> FindUserByTokenAsync(RefreshTokenValue token);

        protected virtual async IAsyncEnumerable<Claim> GetClaimsAsync(TUser user)
        {
            await ValueTask.CompletedTask;

            yield break;
        }

        internal static ValueTask<IList<Claim>> GetStandardClaimsAsync(TUser user)
        {
            var claims = new List<Claim>();

            claims
                .AddClaim(System.Security.Claims.ClaimTypes.PrimarySid, user.Id.Value)
                .AddClaim(ClaimTypes.Username, user.Username.Value)
                .AddClaim(System.Security.Claims.ClaimTypes.Email, user.Email)
                .AddClaim(JwtRegisteredClaimNames.Sub, user.Username.Value) // subject (required)
                .AddClaim(System.Security.Claims.ClaimTypes.Name, user.Name)
                .AddClaim(JwtRegisteredClaimNames.Jti, user.Id.Value.ToString()!) // token id is scoped to the user id
                .AddClaim(ClaimTypes.LastAuthenticationChange, user.LastAuthenticationChange.AddSeconds(1)); // there is some slight precision loss in the string round-trip, so we give it an extra second

            return ValueTask.FromResult((IList<Claim>)claims);
        }

        private async ValueTask SaveUserAsync()
        {
            //let the subclass do it's thing
            await OnSaveUserAsync();

            //commit the current transaction
            await _unitOfWorkFactory.Current.CommitAsync();

            //open a new unit of work in case there are more db calls
            _unitOfWorkFactory.Open();
        }

        protected virtual ValueTask OnSaveUserAsync() => ValueTask.CompletedTask;

        internal virtual async ValueTask<JwtSecurityToken> GenerateAccessTokenAsync(TUser user)
        {
            var claims = await GetStandardClaimsAsync(user);

            //add application claims
            await foreach (var claim in GetClaimsAsync(user))
            {
                claims.Add(claim);
            }

            return await CreateJwtTokenAsync(claims);
        }

        internal ValueTask<JwtSecurityToken> CreateJwtTokenAsync(IEnumerable<Claim> claims) => ValueTask.FromResult<JwtSecurityToken>(new(
                _settings.Issuer,
                _settings.Audience,
                claims,
                expires: SystemDateTime.UtcNow.AddMinutes(_settings.AccessTokenTTLMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(_settings.Secret)), SecurityAlgorithms.HmacSha256)));

        private async ValueTask<(JwtSecurityToken AccessToken, TRefreshToken RefreshToken)> GetTokensAsync(TUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);

            var refreshToken = user
                .AddRefreshToken(
                    SystemDateTime.UtcNow.AddMinutes(_settings.RefreshTokenTTLMinutes),
                    IPAddressValue.Create(_httpContextAccessor.GetIPv4Address()).Value);

            return (accessToken, refreshToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _unitOfWorkFactory?.Dispose();
                }

                _disposed = true;
            }
        }
    }

    public abstract class RESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive> :
        RESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUser : IUser<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
            where TRoleValue : IRoleValue<TRolePrimitive>
            where TRolePrimitive : notnull
    {
        protected RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }

        internal override async ValueTask<JwtSecurityToken> GenerateAccessTokenAsync(TUser user)
        {
            var claims = await GetStandardClaimsAsync(user);

            //add roles
            foreach (var role in user.Roles)
            {
                claims.AddClaim(System.Security.Claims.ClaimTypes.Role, role.Name, true);
            }

            //add application claims
            await foreach (var claim in GetClaimsAsync(user))
            {
                claims.Add(claim);
            }

            return await CreateJwtTokenAsync(claims);
        }
    }
}
