namespace Fanzoo.Kernel.Web.Services.Configuration
{
    public class JwtSecurityTokenSettings
    {
        public string Issuer { get; set; } = default!;

        public string Audience { get; set; } = default!;

        public string Secret { get; set; } = default!;

        public double AccessTokenTTLMinutes { get; set; } = default!;

        public double RefreshTokenTTLMinutes { get; set; } = default!;
    }
}
