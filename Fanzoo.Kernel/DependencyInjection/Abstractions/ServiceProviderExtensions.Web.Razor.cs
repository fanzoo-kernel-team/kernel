using Fanzoo.Kernel.Domain.Entities.Users;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Web.Services;
using Fanzoo.Kernel.Web.Validation.Cookies;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRazorPagesCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this IServiceCollection services)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUser : IUser<TUserIdentifier, TUserIdentifierPrimitive, TUsername>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddControllersWithViews();

            services
                .AddTransient<IPasswordHashingService, IdentityPasswordHashingService>()
                .AddTransient<IRazorPagesUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, TUserAuthenticationService>()
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
