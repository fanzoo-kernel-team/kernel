using System.Text.Json.Serialization;
using Fanzoo.Kernel.Domain.Entities;
using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRESTApiCore<TUserAuthenticationService, TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>(this IServiceCollection services, string jwtPrivateKey)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUser : IUser<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive, TUserIdentifier, TUserIdentifierPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()

        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

#if DEBUG
                    options.JsonSerializerOptions.WriteIndented = true;
#endif

                });

            services
                .AddTransient<IPasswordHashingService, IdentityPasswordHashingService>()
                .AddTransient<IRESTApiUserAuthenticationService<TUser, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>, TUserAuthenticationService>()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //TODO: figure out how to get issuer and audience validation working
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtPrivateKey))
                    };
                });

            return services;
        }
    }
}
