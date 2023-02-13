using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

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

        protected ValueTask StoreTokenDataAsync(string accessToken, string refreshToken) => ValueTask.CompletedTask;
    }

    public class BlazorContextAccessorService : IContextAccessorService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IOptions<JwtSecurityTokenSettings> _jwtSecurityTokenSettings;

        private bool _initialized = false;
        private ClaimsPrincipal? _user = null;


        public BlazorContextAccessorService(ILocalStorageService localStorageService, IOptions<JwtSecurityTokenSettings> jwtSecurityTokenSettings)
        {
            _localStorageService = localStorageService;
            _jwtSecurityTokenSettings = jwtSecurityTokenSettings;
        }

        public ClaimsPrincipal? GetUser() => throw new NotImplementedException(); //synchronous version isn't supported

        public async ValueTask<ClaimsPrincipal?> GetUserAsync()
        {
            if (_initialized)
            {
                return _user;
            }

            if (await _localStorageService.ContainsAsync("accessToken"))
            {
                var handler = new JwtSecurityTokenHandler();

                _user = handler.ValidateToken(await _localStorageService.GetAsync("accessToken"), _jwtSecurityTokenSettings.Value.GetValidationParameters(), out _);
            }

            _initialized = true;

            return _user;
        }
    }

    public interface ILocalStorageService
    {
        ValueTask<string> GetAsync(string key, CancellationToken cancellationToken = default);

        ValueTask SetAsync(string key, string data, CancellationToken cancellationToken = default);

        ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default);

        ValueTask ClearAllAsync(CancellationToken cancellationToken = default);

        ValueTask<bool> ContainsAsync(string key, CancellationToken cancellationToken = default);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask<string> GetAsync(string key, CancellationToken cancellationToken = default) => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);

        public async ValueTask SetAsync(string key, string data, CancellationToken cancellationToken = default) => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);

        public async ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default) => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);

        public async ValueTask ClearAllAsync(CancellationToken cancellationToken = default) => await _jsRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);

        public async ValueTask<bool> ContainsAsync(string key, CancellationToken cancellationToken = default) => await _jsRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key);
    }

    //public sealed class TokenAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    private readonly IContextAccessorService _contextAccessorService;

    //    public TokenAuthenticationStateProvider(IContextAccessorService contextAccessorService)
    //    {
    //        _contextAccessorService = contextAccessorService;
    //    }

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        var user = await _contextAccessorService.GetUserAsync();

    //        return user is not null
    //            ? new AuthenticationState(user)
    //            : new AuthenticationState(new ClaimsPrincipal());
    //    }
    //}

    public sealed class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IOptions<JwtSecurityTokenSettings> _jwtSecurityTokenSettings;

        public TokenAuthenticationStateProvider(ILocalStorageService localStorageService, IOptions<JwtSecurityTokenSettings> jwtSecurityTokenSettings)
        {
            _localStorageService = localStorageService;
            _jwtSecurityTokenSettings = jwtSecurityTokenSettings;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal();

            if (await _localStorageService.ContainsAsync("accessToken"))
            {
                var handler = new JwtSecurityTokenHandler();

                user = handler.ValidateToken(await _localStorageService.GetAsync("accessToken"), _jwtSecurityTokenSettings.Value.GetValidationParameters(), out _);
            }

            return new AuthenticationState(user);
        }
    }
}
