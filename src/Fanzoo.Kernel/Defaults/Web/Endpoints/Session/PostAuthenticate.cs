namespace Fanzoo.Kernel.Defaults.Web.Endpoints.Session
{
    public record struct AuthenticationRequest(string Username, string Password);

    public record struct AuthenticationResponse(string AccessToken, string RefreshToken);

    public static class PostAuthenticate
    {
        public static async Task<Results<Ok<AuthenticationResponse>, BadRequest>> HandleAsync(
            IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue> userAuthenticationService,
            AuthenticationRequest request)
        {
            var response = await userAuthenticationService.AuthenticateAsync(request.Username, request.Password);

            return response.IsSuccessful ? TypedResults.Ok(new AuthenticationResponse(response.Value.AccessToken, response.Value.RefreshToken)) : TypedResults.BadRequest();
        }
    }
}
