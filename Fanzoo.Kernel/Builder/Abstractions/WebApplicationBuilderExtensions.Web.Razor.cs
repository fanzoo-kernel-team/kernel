using Fanzoo.Kernel.Domain.Entities.Users;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRazorPagesCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUser : IUser<TUserIdentifier, TUserIdentifierPrimitive, TUsername>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue

        {
            builder.Services
                .AddWebCore()
                .AddRazorPagesCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>();

            return builder;
        }
    }
}

