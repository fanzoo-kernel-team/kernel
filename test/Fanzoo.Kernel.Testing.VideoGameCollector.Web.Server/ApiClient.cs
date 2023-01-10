namespace Fanzoo.Kernel.Testing.VideoGameCollector.Web.Server
{
    public interface IApiClientHost
    {
        void ConfigureServices(IServiceCollection services);
    }

    public abstract class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _name;

        protected ApiClient(IHttpClientFactory httpClientFactory, string name)
        {
            _httpClientFactory = httpClientFactory;
            _name = name;
        }

        protected HttpClient Client => _httpClientFactory.CreateClient(_name);
    }
}
