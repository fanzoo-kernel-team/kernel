namespace Fanzoo.Kernel.Defaults.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddRESTApiCore<TUserAuthenticationService>(this WebApplicationBuilder builder)
            where TUserAuthenticationService : class, IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>
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
                .AddRESTApiCore<TUserAuthenticationService, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue>(settings);

            return builder;
        }
    }
}