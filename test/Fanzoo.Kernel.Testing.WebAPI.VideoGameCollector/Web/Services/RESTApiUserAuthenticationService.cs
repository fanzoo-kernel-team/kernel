using Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens;
using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Values;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Data.Repositories;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services
{
    public class RESTApiUserAuthenticationService : Fanzoo.Kernel.Defaults.Web.Services.RESTApiUserAuthenticationService<ApplicationRoleValue>
    {
        private readonly IUserRepository _userRepository;

        public RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository) : base(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory)
        {
            _userRepository = userRepository;
        }

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByTokenAsync(RefreshTokenValue token) => throw new NotImplementedException();

        protected override async ValueTask<User<ApplicationRoleValue>?> FindUserByUsernameAsync(EmailUsernameValue username) => await _userRepository.FindByUsername(username);

        protected override UserIdentifierValue? GetClaimIdentifierOrDefault(string? claimValue) => throw new NotImplementedException();
    }
}
