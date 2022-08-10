namespace Fanzoo.Kernel.Stripe.Services.Configuration
{
    public class StripeSettings
    {
        public string ApiKey { get; set; } = default!;

        public string PublishableKey { get; set; } = default!;

        public string WebhookSigningSecret { get; set; } = default!;
    }
}
