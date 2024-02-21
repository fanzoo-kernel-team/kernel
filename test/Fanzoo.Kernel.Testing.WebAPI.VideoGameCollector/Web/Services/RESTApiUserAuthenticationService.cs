using Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens;
using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services
{
    public class RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository) : Defaults.Web.Services.RESTApiUserAuthenticationService<ApplicationRoleValue>(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory)
    {
        private readonly IUserRepository _userRepository = userRepository;

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override async ValueTask<User<ApplicationRoleValue>?> FindUserByTokenAsync(RefreshTokenValue token) => await _userRepository.FindByRefreshToken(token);

        protected override async ValueTask<User<ApplicationRoleValue>?> FindUserByUsernameAsync(EmailUsernameValue username) => await _userRepository.FindByUsername(username);

        protected override UserIdentifierValue? FindClaimIdentifier(string? claimValue) => throw new NotImplementedException();
    }
}
