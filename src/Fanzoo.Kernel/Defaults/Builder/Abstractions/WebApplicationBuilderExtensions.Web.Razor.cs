namespace Fanzoo.Kernel.Defaults.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRazorPagesCore<TUserAuthenticationService>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>, ICookieUserAuthenticationService
        {
            builder.Services
                .AddWebCore()
                .AddRazorPagesCore<TUserAuthenticationService, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>();

            return builder;
        }
    }
}