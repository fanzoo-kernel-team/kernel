using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Defaults.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRESTApiCore<TUserAuthenticationService>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>
        {
            builder.Services
                .AddWebCore()
                .AddRESTApiCore<TUserAuthenticationService, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>(builder.Configuration["Jwt:Secret"] ?? throw new ArgumentException("Configuration not found."));

            return builder;
        }
    }
}