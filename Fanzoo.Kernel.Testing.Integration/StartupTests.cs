using System.Threading.Tasks;
using Fanzoo.Kernel.SendGrid.Services;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Stripe.Services;
using Xunit;

namespace Fanzoo.Kernel.Testing.Integration
{
    [Collection("LocalDb")]
    public class StartupTests
    {
        private readonly IntegrationSqlLocalDbWebApplicationFactory _factory;

        public StartupTests(IntegrationSqlLocalDbWebApplicationFactory factory)
        {
            _factory = factory;
        }

        //This test should run first
        [Fact, Priority(0)]
        public async Task Application_Dependency_Injection_And_StartUp() =>
            (await _factory
                .CreateClient()
                    .GetAsync("/heartbeat"))
                        .EnsureSuccessStatusCode();

        [Fact]
        public void SendGrid_Service_Loads_On_Startup()
        {
            var emailService = _factory.Services.GetService(typeof(IEmailService));

            Assert.NotNull(emailService);
            Assert.IsType<SendGridEmailService>(emailService);
        }

        [Fact]
        public void Stripe_Service_Loads_On_Startup()
        {
            var paymentService = _factory.Services.GetService(typeof(IPaymentService<StripePaymentRequest, StripePaymentResult, StripeCreateCustomerRequest, StripeCreateCustomerResult, StripeCancelPaymentRequest, object?>));

            Assert.NotNull(paymentService);
            Assert.IsType<StripePaymentService>(paymentService);
        }
    }
}