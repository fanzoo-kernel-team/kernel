using Microsoft.AspNetCore.Routing;

namespace Fanzoo.Kernel.Builder
{
    public interface IApplicationModule
    {
        IServiceCollection RegisterServices(IServiceCollection services);

        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
