using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRESTApiCore(this IServiceCollection services) => services.AddCors();

        public static IServiceCollection AddRESTApiCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this IServiceCollection services, string jwtPrivateKey, double clockSkewMinutes, Action<AddMvcCoreConfiguration>? options = null)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            var configuration = new AddMvcCoreConfiguration();

            options?.Invoke(configuration);

            services
                .AddMvcCore(configuration)
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
                        ClockSkew = TimeSpan.FromMinutes(clockSkewMinutes),
                        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                        {
                            if (expires is null)
                            {
                                return false;
                            }

                            expires = expires.Value.Add(validationParameters.ClockSkew.Negate());

                            return expires > SystemDateTime.UtcNow;
                        },
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

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter Access Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new List<string>()
                    }
                });

                var schemaHelper = new SwashbuckleSchemaHelper();

                c.CustomSchemaIds(type => schemaHelper.GetSchemaId(type));
            });

            return services;
        }

        internal class SwashbuckleSchemaHelper
        {
            private readonly Dictionary<string, int> _schemaNameRepetition = [];

            // borrowed from https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/95cb4d370e08e54eb04cf14e7e6388ca974a686e/src/Swashbuckle.AspNetCore.SwaggerGen/SchemaGenerator/SchemaGeneratorOptions.cs#L44
            private string DefaultSchemaIdSelector(Type modelType)
            {
                if (!modelType.IsConstructedGenericType)
                {
                    return modelType.Name.Replace("[]", "Array");
                }

                var prefix = modelType.GetGenericArguments()
                    .Select(DefaultSchemaIdSelector)
                        .Aggregate((previous, current) => previous + current);

                return $"{prefix}{modelType.Name.Split('`')[0]}";
            }

            public string GetSchemaId(Type modelType)
            {
                var id = DefaultSchemaIdSelector(modelType);

                if (!_schemaNameRepetition.ContainsKey(id))
                {
                    _schemaNameRepetition.Add(id, 0);
                }

                var count = _schemaNameRepetition[id] + 1;

                _schemaNameRepetition[id] = count;

                return $"{id}{(count > 1 ? count.ToString() : "")}";
            }
        }
    }
}
