using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities;
using Fanzoo.Kernel.Web.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Endpoints
{
    public record struct AuthenticationRequest(string Username, string Password);

    public static class PostAuthenticate
    {
        public static async Task<Results<Ok<string>, BadRequest>> HandleAsync(
            [FromServices] IRESTApiUserAuthenticationService<User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, Domain.Entities.RefreshTokens.Guid.RefreshToken, RefreshTokenIdentifierValue, Guid> userAuthenticationService,
            [FromBody] AuthenticationRequest request)
        {
            var response = await userAuthenticationService.AuthenticateAsync(request.Username, request.Password);

            return response.IsSuccessful ? TypedResults.Ok(response.Value.AccessToken) : TypedResults.BadRequest();
        }
    }
}
