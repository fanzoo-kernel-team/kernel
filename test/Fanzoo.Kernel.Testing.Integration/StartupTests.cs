using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fanzoo.Kernel.Defaults.Web.Endpoints.Session;
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

        [Fact]
        public async Task Application_Dependency_Injection_And_StartUp() =>
            (await _factory
                .CreateClient()
                    .GetAsync("/heartbeat"))
                        .EnsureSuccessStatusCode();

        [Fact]
        public async Task Test_Can_Create_Game()
        {
            var (client, _) = await LoginAsync("billw@fanzootechnology.com", "Test123!");

            (await client
                    .PostAsync("/games?name=Pitfall", null))
                        .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Test_Can_Update_Many()
        {
            var (client, _) = await LoginAsync("billw@fanzootechnology.com", "Test123!");

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

        [Fact]
        public async Task Test_Must_Be_In_Administrator_Role()
        {
            var (client, _) = await LoginAsync("bob@fanzootechnology.com", "Test123!");

            var result = await client.GetAsync("/requires-administrator-role");

            Assert.True(result.StatusCode == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Test_Can_Refresh_Token()
        {
            var (client, _) = await LoginAsync("billw@fanzootechnology.com", "Test123!");

            //wait a few seconds for the token to expire
            Thread.Sleep(2000);

            //make sure the old one fails
            var result = await client.GetAsync("/requires-administrator-role");

            Assert.True(!result.IsSuccessStatusCode);

            AuthenticationResponse response;

            //re-authenticate
            (client, response) = await LoginAsync("billw@fanzootechnology.com", "Test123!");

            //refresh the token
            result = await client.PostAsJsonAsync("/session/tokens/refresh", new RefreshTokenRequest(response.RefreshToken));

            //get a new set of tokens
            var tokenResponse = await result.Content.ReadFromJsonAsync<RefreshTokenResponse>();

            //reset the header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            //make sure it works now
            result = await client.GetAsync("/requires-administrator-role");

            result.EnsureSuccessStatusCode();

            //make sure we can't reuse the refresh token
            result = await client.PostAsJsonAsync("/session/tokens/refresh", new RefreshTokenRequest(response.RefreshToken));

            Assert.True(!result.IsSuccessStatusCode);

        }

        private async Task<(HttpClient Client, AuthenticationResponse Response)> LoginAsync(string username, string password)
        {
            var client = _factory.CreateClient();

            var result = await client.PostAsJsonAsync("/session/authenticate", new AuthenticationRequest(username, password));

            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadFromJsonAsync<AuthenticationResponse>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);

            return (client, response);
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