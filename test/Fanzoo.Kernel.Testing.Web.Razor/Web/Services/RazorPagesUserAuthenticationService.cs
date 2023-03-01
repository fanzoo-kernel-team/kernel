using Fanzoo.Kernel.Defaults.Domain.Entities.Users;
using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;

namespace Fanzoo.Kernel.Testing.Web.Razor.Web.Services
{
    public class RazorPagesUserAuthenticationService : Defaults.Web.Services.RazorPagesUserAuthenticationService<ApplicationRoleValue>
    {
        private readonly IUserRepository _userRepository;

        public RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository) : base(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
        {
            _userRepository = userRepository;
        }

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByIdAsync(UserIdentifierValue identifier) => throw new NotImplementedException();

        protected override ValueTask<User<ApplicationRoleValue>?> FindUserByUsernameAsync(EmailUsernameValue username) => throw new NotImplementedException();

        protected override UserIdentifierValue? FindClaimIdentifier(string? claimValue) => throw new NotImplementedException();

    }
}
