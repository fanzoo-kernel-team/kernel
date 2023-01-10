using System.Net;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Fanzoo.Kernel.Testing.VideoGameCollector.Web.Server.Modules.Session
{
    public class SessionClientHost : IApiClientHost
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //configure http factory with polly policy

            services.AddHttpClient(SessionClient.Name, client =>
            {
                client.BaseAddress = new Uri("http://localhost:5043"); //TODO: read from config
            })
                .AddPolicyHandler(Policy<HttpResponseMessage> //TODO: add some default policies
                    .Handle<HttpRequestException>()
                    .OrResult(r => r.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
                        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5))); //TODO: add short-circuiting 

            services.AddSingleton<ISessionClient, SessionClient>();
        }
    }

    public interface ISessionClient
    {
        ValueTask<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }

    public sealed partial class SessionClient : ApiClient, ISessionClient
    {
        public const string Name = "session";

        public SessionClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, Name) { }
    }
}
