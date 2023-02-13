using Microsoft.IdentityModel.Tokens;

namespace Fanzoo.Kernel.Web.Services.Configuration
{
    public class JwtSecurityTokenSettings
    {
        public double ClockSkewMinutes { get; set; } = default!;

        public string Issuer { get; set; } = default!;

        public string Audience { get; set; } = default!;

        public string Secret { get; set; } = default!;

        public double AccessTokenTTLMinutes { get; set; } = default!;

        public double RefreshTokenTTLMinutes { get; set; } = default!;
    }

    public static class JwtSecurityTokenSettingsExtensions
    {
        public static TokenValidationParameters GetValidationParameters(this JwtSecurityTokenSettings settings) => new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.FromMinutes(settings.ClockSkewMinutes),
            LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
            {
                if (expires is null)
                {
                    return false;
                }

                expires = expires.Value.Add(validationParameters.ClockSkew.Negate());

                return expires > SystemDateTime.Now;
            },
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(settings.Secret))
        };
    }
}
