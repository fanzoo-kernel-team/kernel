using Fanzoo.Kernel.Defaults.Domain.Entities.Users;
using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;

namespace Fanzoo.Kernel.Testing.Web.Razor.Web.Services
{
    public class RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository) : Defaults.Web.Services.RazorPagesUserAuthenticationService<ApplicationRoleValue>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
    {
        private readonly IUserRepository _userRepository = userRepository;

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByUsernameAsync(EmailUsernameValue username) => throw new NotImplementedException();

        protected override UserIdentifierValue? FindClaimIdentifier(string? claimValue) => throw new NotImplementedException();

    }
}
