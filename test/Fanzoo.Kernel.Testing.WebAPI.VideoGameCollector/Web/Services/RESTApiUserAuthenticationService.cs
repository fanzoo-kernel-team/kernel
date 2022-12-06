using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Guid;
using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Data.Repositories;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services
{
    public class RESTApiUserAuthenticationService : Kernel.Web.Services.RESTApiUserAuthenticationService<Modules.Users.Core.Entities.User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>
    {
        private readonly IUserRepository _userRepository;

        public RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository) : base(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory)
        {
            _userRepository = userRepository;
        }

        protected override UserIdentifierValue? GetClaimIdentifierOrDefault(string? claimValue) => throw new NotImplementedException();

        protected override ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> FindUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> FindUserByTokenAsync(RefreshTokenValue token) => throw new NotImplementedException();

        protected override async ValueTask<IUser<UserIdentifierValue, Guid, EmailUsernameValue, RefreshToken, RefreshTokenIdentifierValue, Guid>?> FindUserByUsernameAsync(EmailUsernameValue username) => await _userRepository.FindByUsername(username);

    }
}
