using Fanzoo.Kernel.Web.Validation.Cookies;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.DependencyInjection
{
    public class AddRazorPagesCoreOptions
    {
        public bool DisableBuiltInValidation { get; set; } = false;
    }

    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRazorPagesCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this IServiceCollection services, Action<AddRazorPagesCoreOptions>? options = null)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            var razorPagesOptions = new AddRazorPagesCoreOptions();

            options?.Invoke(razorPagesOptions);

            services.AddMvc(o =>
            {
                if (razorPagesOptions.DisableBuiltInValidation)
                {
                    o.ModelValidatorProviders.Clear();
                }
            });

            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            services
                .AddTransient<IPasswordHashingService, IdentityPasswordHashingService>()
                .AddTransient<IRazorPagesUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, TUserAuthenticationService>()
                .AddTransient<ICookieUserAuthenticationService, TUserAuthenticationService>()
                .AddScoped<PersistentCookieAuthenticationEvents>()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromDays(90);
                        options.SlidingExpiration = true;
                        options.EventsType = typeof(PersistentCookieAuthenticationEvents);
                    });

            return services;
        }
    }
}
