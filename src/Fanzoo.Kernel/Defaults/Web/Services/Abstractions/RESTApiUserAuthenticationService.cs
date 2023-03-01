#pragma warning disable S101 // Types should be named in PascalCase

using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Defaults.Web.Services
{
    public abstract class RESTApiUserAuthenticationService :
        RESTApiUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>
    {
        protected RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }
    }

    public abstract class RESTApiUserAuthenticationService<TRoleValue> :
        RESTApiUserAuthenticationService<User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, TRoleValue, Guid, RefreshToken, RefreshTokenIdentifierValue, Guid>
            where TRoleValue : IRoleValue<Guid>
    {
        protected RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) : base(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory) { }
    }
}
