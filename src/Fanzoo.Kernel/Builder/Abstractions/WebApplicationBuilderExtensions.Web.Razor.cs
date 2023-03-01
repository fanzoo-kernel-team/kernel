namespace Fanzoo.Kernel.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRazorPagesCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue

        {
            builder.Services
                .AddWebCore()
                .AddRazorPagesCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>();

            return builder;
        }
    }
}

