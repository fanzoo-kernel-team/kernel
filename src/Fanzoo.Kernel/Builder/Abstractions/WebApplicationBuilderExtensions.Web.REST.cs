using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;
using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRESTApiCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUser : IUser<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
        {
            //TODO: is there an alternate way to get the secret key from the config without hard-coded values??

            builder.Services
                .AddWebCore()
                .AddRESTApiCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>(builder.Configuration["Jwt:Secret"] ?? throw new ArgumentException("Configuration not found."));

            return builder;
        }
    }
}