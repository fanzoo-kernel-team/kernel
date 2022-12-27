using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Defaults.Web.Endpoints.Session;

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

            return endpoints;
        }

        public IServiceCollection RegisterServices(IServiceCollection services) => services;
    }
}
