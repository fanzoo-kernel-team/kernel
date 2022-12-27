namespace Fanzoo.Kernel.Defaults.Web.Endpoints.Session
{
    public record struct RefreshTokenRequest(string RefreshToken);

    public record struct RefreshTokenResponse(string AccessToken, string RefreshToken);

    public static class PostRefreshToken
    {
        public static async Task<Results<Ok<RefreshTokenResponse>, BadRequest>> HandleAsync(
            IRESTApiUserAuthenticationService<UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue> userAuthenticationService,
            RefreshTokenRequest request)
        {
            var result = await userAuthenticationService.RefreshTokenAsync(request.RefreshToken);

            return result.IsSuccessful ? TypedResults.Ok(new RefreshTokenResponse(result.Value.AccessToken, result.Value.RefreshToken)) : TypedResults.BadRequest();
        }
    }
}
