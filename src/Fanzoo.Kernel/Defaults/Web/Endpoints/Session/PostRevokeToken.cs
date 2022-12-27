namespace Fanzoo.Kernel.Defaults.Web.Endpoints.Session
{
    public record struct RevokeTokenRequest(string RefreshToken);

    public static class PostRevokeToken
    {
        public static async Task<Results<Ok, BadRequest>> HandleAsync(
            IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue> userAuthenticationService,
            RevokeTokenRequest request)
        {
            var result = await userAuthenticationService.RevokeAsync(request.RefreshToken);

            return result.IsSuccessful ? TypedResults.Ok() : TypedResults.BadRequest();
        }
    }
}
