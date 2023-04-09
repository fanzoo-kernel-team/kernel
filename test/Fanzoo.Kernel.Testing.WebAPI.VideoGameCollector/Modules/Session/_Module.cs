using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Defaults.Web.Endpoints.Session;
using Fanzoo.Kernel.Web.Services;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Session
{
    public class UserModule : IApplicationModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var group = endpoints
                .MapGroup("/session")
                    .RequireAuthorization()
                    .WithTags("Session");

            group
                .MapPost("/authenticate", PostAuthenticate.HandleAsync)
                    .AllowAnonymous();

            group.MapPost("/tokens/refresh", PostRefreshToken.HandleAsync);

            group.MapPost("/tokens/revoke", PostRevokeToken.HandleAsync);

            group.MapGet("email", (ICurrentUserService currentUserService) =>
            {
                return currentUserService.GetEmailOrDefault();
            });

            return endpoints;
        }

        public IServiceCollection RegisterServices(IServiceCollection services) => services;
    }
}
