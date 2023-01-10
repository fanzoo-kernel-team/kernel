namespace Fanzoo.Kernel.Testing.VideoGameCollector.Web.Server.Modules.Session
{
    public record AuthenticationRequest(string? Username, string? Password);

    public record AuthenticationResponse(string AccessToken, string RefreshToken);

    public sealed partial class SessionClient
    {
        public async ValueTask<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var result = await Client.PostAsJsonAsync("/session/authenticate", request);

            return await result.Content.ReadFromJsonAsync<AuthenticationResponse>() ?? throw new InvalidOperationException();
        }
    }
}