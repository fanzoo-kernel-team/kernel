using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Stripe.Services.Configuration;
using Microsoft.Extensions.Options;
using Stripe;

namespace Fanzoo.Kernel.Stripe.Services
{
    public sealed class StripePaymentService : IPaymentService<StripePaymentRequest, StripePaymentResult, StripeCreateCustomerRequest, StripeCreateCustomerResult, StripeCancelPaymentRequest, object?>
    {
        private readonly IOptions<StripeSettings> _settings;

        public StripePaymentService(IOptions<StripeSettings> settings)
        {
            _settings = settings;
        }

        public async ValueTask<StripeCreateCustomerResult> CreateCustomerAsync(StripeCreateCustomerRequest request)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Description = request.Description,
                Email = request.Email,
                Metadata = request.Metadata
            };

            var customerService = new CustomerService();

            var customer = await customerService.CreateAsync(customerOptions, new RequestOptions { ApiKey = _settings.Value.ApiKey });

            return new(customer.Id);
        }

        public async ValueTask<StripePaymentResult> CreatePaymentAsync(StripePaymentRequest request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Customer = request.CustomerId
            };

            var paymentService = new PaymentIntentService();

            var paymentIntent = await paymentService.CreateAsync(options, new RequestOptions { ApiKey = _settings.Value.ApiKey });

            return new(paymentIntent.Id, paymentIntent.ClientSecret);
        }

        public async ValueTask<object?> CancelPaymentAsync(StripeCancelPaymentRequest request)
        {
            var options = new PaymentIntentCancelOptions
            {
                CancellationReason = "abandoned"
            };

            var paymentService = new PaymentIntentService();

            _ = await paymentService.CancelAsync(request.PaymentId, options, new RequestOptions { ApiKey = _settings.Value.ApiKey });

            return default;
        }
    }

    public record StripeCreateCustomerRequest(string Description, string Email, Dictionary<string, string> Metadata);

    public record StripeCreateCustomerResult(string CustomerId);

    public record StripePaymentRequest(string CustomerId, long Amount, string Currency);

    public record StripePaymentResult(string Id, string ClientSecret);

    public record StripeCancelPaymentRequest(string PaymentId);
}
