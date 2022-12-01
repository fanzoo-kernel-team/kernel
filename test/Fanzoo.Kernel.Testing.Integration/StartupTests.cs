using System.Net.Http.Json;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries;
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
        public async Task Test_Can_Create_Game() => (await _factory
                .CreateClient()
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();

        [Fact]
        public async Task Test_Can_Update_Many()
        {
            (await _factory
                .CreateClient()
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();

            (await _factory
                .CreateClient()
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();

            (await _factory
                .CreateClient()
                    .PutAsJsonAsync("/games/rename", new RenameAllRequest("Pitfall", "Pitfall!")))
                        .EnsureSuccessStatusCode();

            var details = await (await _factory
                .CreateClient()
                    .GetAsync("/games?name=Pitfall!"))
                        .Content.ReadFromJsonAsync<IEnumerable<GameDetailResult>>();

            Assert.NotNull(details);

            Assert.True(details.Count() > 1);
        }

        //TODO: move these to the new solution files when complete
        //[Fact]
        //public void SendGrid_Service_Loads_On_Startup()
        //{
        //    var emailService = _factory.Services.GetService(typeof(IEmailService));

        //    Assert.NotNull(emailService);
        //    Assert.IsType<SendGridEmailService>(emailService);
        //}

        //[Fact]
        //public void Stripe_Service_Loads_On_Startup()
        //{
        //    var paymentService = _factory.Services.GetService(typeof(IPaymentService<StripePaymentRequest, StripePaymentResult, StripeCreateCustomerRequest, StripeCreateCustomerResult, StripeCancelPaymentRequest, object?>));

        //    Assert.NotNull(paymentService);
        //    Assert.IsType<StripePaymentService>(paymentService);
        //}
    }
}