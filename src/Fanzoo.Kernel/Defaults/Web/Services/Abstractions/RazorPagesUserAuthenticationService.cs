using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Defaults.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Web.Services
{
    public abstract class RazorPagesUserAuthenticationService :
        RazorPagesUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>
    {
        protected RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }
    }

    public abstract class RazorPagesUserAuthenticationService<TRoleValue> :
        RazorPagesUserAuthenticationService<User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>
            where TRoleValue : IRoleValue<Guid>
    {
        protected RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }
    }
}

namespace Fanzoo.Kernel.Defaults.Web.Services.RefreshTokens
{
    public abstract class RazorPagesUserAuthenticationService<TRoleValue> :
        RazorPagesUserAuthenticationService<Domain.Entities.Users.RefreshTokens.User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, TRoleValue, Guid, Domain.Entities.Users.RefreshTokens.RefreshToken, RefreshTokenIdentifierValue, Guid>
            where TRoleValue : IRoleValue<Guid>
    {
        protected RazorPagesUserAuthenticationService(IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }
    }
}
