using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Defaults.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Web.Services
{
    public abstract class RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RazorPagesUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
    {
    }

    public abstract class RazorPagesUserAuthenticationService<TRoleValue>(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RazorPagesUserAuthenticationService<User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
            where TRoleValue : IRoleValue<Guid>
    {
    }
}

namespace Fanzoo.Kernel.Defaults.Web.Services.RefreshTokens
{
    public abstract class RazorPagesUserAuthenticationService<TRoleValue>(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RazorPagesUserAuthenticationService<Domain.Entities.Users.RefreshTokens.User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, TRoleValue, Guid, Domain.Entities.Users.RefreshTokens.RefreshToken, RefreshTokenIdentifierValue, Guid>(httpContextAccessor, passwordHashingService, unitOfWorkFactory)
            where TRoleValue : IRoleValue<Guid>
    {
    }
}
