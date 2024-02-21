using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Fanzoo.Kernel.Web.Mvc
{
    [ApiController]
    public abstract class ApiController(CommandDispatcher commandDispatcher, QueryDispatcher queryDispatcher) : ControllerBase
    {
        protected CommandDispatcher CommandDispatcher { get; init; } = commandDispatcher;

        protected QueryDispatcher QueryDispatcher { get; init; } = queryDispatcher;
    }
}
