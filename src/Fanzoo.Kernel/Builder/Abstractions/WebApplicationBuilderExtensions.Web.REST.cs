using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRESTApiCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            //TODO: is there an alternate way to get the secret key from the config without hard-coded values??

            builder.Services
                .AddWebCore()
                .AddRESTApiCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(builder.Configuration["Jwt:Secret"] ?? throw new ArgumentException("Configuration not found."));

            return builder;
        }
    }
}