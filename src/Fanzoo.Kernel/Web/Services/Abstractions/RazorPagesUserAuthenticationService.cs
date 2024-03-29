﻿using System.Globalization;
using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.Web.Services
{
    public interface IRazorPagesUserAuthenticationService<TIdentifier, TPrimitive, TUsername, TPassword>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TPassword : IPasswordValue
    {
        ValueTask<UnitResult<Error>> SignInAsync(TUsername username, TPassword password);

        ValueTask SignOutAsync(TIdentifier identifier);

        public Task ValidateLastAuthenticationChangeAsync(CookieValidatePrincipalContext context);

    }

    public abstract class RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword>(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : IDisposable, IAsyncDisposable,
        IRazorPagesUserAuthenticationService<TIdentifier, TPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUser : IUser<TIdentifier, TPrimitive, TUsername>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
    {
        private readonly IPasswordHashingService _passwordHashingService = passwordHashingService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;

        private bool _disposed = false;

        public async ValueTask<UnitResult<Error>> SignInAsync(TUsername username, TPassword password)
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

            var claims = await GetStandardClaimsAsync(user);

            //add internal claims
            await foreach (var claim in GetClaimsInternalAsync(user))
            {
                claims.Add(claim);
            }

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

            _unitOfWorkFactory.Close();

            return UnitResult.Success<Error>();
        }

        public async ValueTask SignOutAsync(TIdentifier identifier)
        {
            _unitOfWorkFactory.Open();

            var user = await FindUserByIdAsync(identifier);

            if (user is null)
            {
                return;
            }

            user.SignOut();

            await _httpContextAccessor
                .HttpContext!
                    .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await SaveUserAsync();

            _unitOfWorkFactory.Close();
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
            _unitOfWorkFactory.Open();

            var identifier = FindClaimIdentifier(principal.Claims.GetClaimValueOrDefault(System.Security.Claims.ClaimTypes.PrimarySid));

            if (identifier is not null)
            {
                var user = await FindUserByIdAsync(identifier);

                if (user is not null)
                {
                    var lastAuthenticationChange = principal.Claims.GetClaimValueOrDefault(ClaimTypes.LastAuthenticationChange);

                    if (lastAuthenticationChange is not null
                        && DateTime.TryParse(lastAuthenticationChange, new CultureInfo("en-US"), out var lastAuthenticationChangeDate)
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

        protected virtual async IAsyncEnumerable<Claim> GetClaimsAsync(TUser user)
        {
            await ValueTask.CompletedTask;

            yield break;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "Different scope")]
        protected internal virtual async IAsyncEnumerable<Claim> GetClaimsInternalAsync(TUser user)
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
                .AddClaim(System.Security.Claims.ClaimTypes.Name, user.Name)
                .AddClaim(JwtRegisteredClaimNames.Sub, user.Username.Value) // subject (required)
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
        }

        protected virtual ValueTask OnSaveUserAsync() => ValueTask.CompletedTask;

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

    public abstract class RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRoleValue, TRolePrimitive>(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
            where TUser : IUser<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRoleValue : IRoleValue<TRolePrimitive>
            where TRolePrimitive : notnull
    {
        protected internal override async IAsyncEnumerable<Claim> GetClaimsInternalAsync(TUser user)
        {
            //add roles
            foreach (var role in user.Roles)
            {
                yield return await Task.Run(() => new Claim(System.Security.Claims.ClaimTypes.Role, role.Name));
            }
        }
    }

    public abstract class RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive>(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRoleValue, TRolePrimitive>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
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
    }
}
