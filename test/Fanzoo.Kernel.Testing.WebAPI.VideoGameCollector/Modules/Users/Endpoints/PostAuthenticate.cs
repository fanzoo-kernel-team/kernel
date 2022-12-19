using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Endpoints
{
    public record struct AuthenticationRequest(string Username, string Password);

    public static class PostAuthenticate
    {
        public static async Task<Results<Ok<string>, BadRequest>> HandleAsync(
            [FromServices] IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue> userAuthenticationService,
            [FromBody] AuthenticationRequest request)
        {
            var response = await userAuthenticationService.AuthenticateAsync(request.Username, request.Password);

            return response.IsSuccessful ? TypedResults.Ok(response.Value.AccessToken) : TypedResults.BadRequest();
        }
    }
}
