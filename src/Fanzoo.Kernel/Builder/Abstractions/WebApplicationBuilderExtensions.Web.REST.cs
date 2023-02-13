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
            var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new ArgumentException("Configuration not found.");
            var clockSkewMinutes = double.Parse(builder.Configuration["Jwt:ClockSkewMinutes"] ?? throw new ArgumentException("Configuration not found."));

            var settings = new JwtSecurityTokenSettings
            {
                Secret = jwtSecret,
                ClockSkewMinutes = clockSkewMinutes
            };

            builder.Services
                .AddWebCore()
                .AddRESTApiCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(settings);

            return builder;
        }

        public static WebApplicationBuilder AddRESTApiCore(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddWebCore()
                .AddRESTApiCore();

            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger();

            return builder;
        }
    }
}