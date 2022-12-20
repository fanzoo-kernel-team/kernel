using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users
{
    public class HomeModule : IApplicationModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/").WithTags("Home");

            group.MapGet("/heartbeat", () =>
            {
                return Results.Ok();
            });

            group.MapGet("/requires-administrator-role", () =>
            {
                return Results.Ok();
            })
                .RequireAuthorization(policy => policy.RequireRole(StringCatalog.Roles.Administrator));

            group.MapPost("/post-test", PostCreate.HandleAsync);

            return endpoints;
        }

        public IServiceCollection RegisterServices(IServiceCollection services) => services;
    }
}
