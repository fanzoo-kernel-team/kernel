using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games
{
    public class GameModule : IApplicationModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/games");

            group.MapGet(string.Empty, GetByName.HandleAsync);

            group.MapPost(string.Empty, PostCreate.HandleAsync);

            group.MapPut("/rename", PutRenameAll.HandleAsync);

            return endpoints;
        }

        public IServiceCollection RegisterServices(IServiceCollection services) => services;
    }
}
