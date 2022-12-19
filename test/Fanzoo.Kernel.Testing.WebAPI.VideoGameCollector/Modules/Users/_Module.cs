using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Endpoints;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users
{
    public class UserModule : IApplicationModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/account").WithTags("Account");

            group.MapPost("/authenticate", PostAuthenticate.HandleAsync)
                .Produces(StatusCodes.Status200OK, contentType: "text/plain");

            return endpoints;
        }

        public IServiceCollection RegisterServices(IServiceCollection services) => services;
    }
}
