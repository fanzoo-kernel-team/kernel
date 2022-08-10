using System.Security.Claims;
using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services
{
    public class RESTApiUserAuthenticationService : Kernel.Web.Services.RESTApiUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>
    {
        public RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService) : base(settings, httpContextAccessor, passwordHashingService)
        {
        }

        protected override UserIdentifierValue? GetClaimIdentifier(string? claimValue) => throw new NotImplementedException();

        protected override IAsyncEnumerable<Claim> GetClaimsAsync(IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid> user) => throw new NotImplementedException();

        protected override ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> GetUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> GetUserByTokenAsync(RefreshTokenValue token) => throw new NotImplementedException();

        protected override ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> GetUserByUsernameAsync(EmailUsernameValue username) => throw new NotImplementedException();

        protected override ValueTask SaveUserAsync() => throw new NotImplementedException();
    }
}
