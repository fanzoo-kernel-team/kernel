#pragma warning disable S101 // Types should be named in PascalCase

using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens;
using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Defaults.Web.Services
{
    public abstract class RESTApiUserAuthenticationService(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RESTApiUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory)
    {
    }

    public abstract class RESTApiUserAuthenticationService<TRoleValue>(IOptions<JwtSecurityTokenSettings> settings, IHttpContextAccessor httpContextAccessor, IPasswordHashingService passwordHashingService, IUnitOfWorkFactory unitOfWorkFactory) :
        RESTApiUserAuthenticationService<User<TRoleValue>, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, TRoleValue, Guid, RefreshToken, RefreshTokenIdentifierValue, Guid>(settings, httpContextAccessor, passwordHashingService, unitOfWorkFactory)
            where TRoleValue : IRoleValue<Guid>
    {
    }
}
