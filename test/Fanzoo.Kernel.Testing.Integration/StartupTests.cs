using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Endpoints;
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

        [Fact]
        public async Task Application_Dependency_Injection_And_StartUp() =>
            (await _factory
                .CreateClient()
                    .GetAsync("/heartbeat"))
                        .EnsureSuccessStatusCode();

        [Fact]
        public async Task Test_Can_Create_Game()
        {
            using var client = await LoginAsync("billw@fanzootechnology.com", "Test123!");

            (await client
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Test_Can_Update_Many()
        {
            using var client = await LoginAsync("billw@fanzootechnology.com", "Test123!");

            (await client
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();

            (await client
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();

            (await client
                    .PutAsJsonAsync("/games/rename", new RenameAllRequest("Pitfall", "Pitfall!")))
                        .EnsureSuccessStatusCode();

            var details = await (await client
                    .GetAsync("/games?name=Pitfall!"))
                        .Content.ReadFromJsonAsync<IEnumerable<GameDetailResult>>();

            Assert.NotNull(details);

            Assert.True(details.Count() > 1);
        }

        private async Task<HttpClient> LoginAsync(string username, string password)
        {
            var client = _factory.CreateClient();

            var result = await client.PostAsJsonAsync("/account/authenticate", new AuthenticationRequest(username, password));

            result.EnsureSuccessStatusCode();

            var token = await result.Content.ReadAsStringAsync();

            //responses are always considered json and the plain text string has quotes added
            //this only happens when running tests and is handled properly elsewhere
            //furthermore the test application is lazy and only returning a string for the token whereas a real-world application would not
            //the result is this code

            token = token.Replace("\"", "");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
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