using Fanzoo.Kernel.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRESTApiCore(this IServiceCollection services)
        {
            return services.AddCors();
        }

        public static IServiceCollection AddRESTApiCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this IServiceCollection services, string jwtPrivateKey)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            services
                .AddTransient<IPasswordHashingService, IdentityPasswordHashingService>()
                .AddTransient<IRESTApiUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, TUserAuthenticationService>()
                .AddCors()
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtPrivateKey))
                    };

#if DEBUG
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                    };
#endif
                });

            return services;
        }
    }
}
