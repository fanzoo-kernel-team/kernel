using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Fanzoo.Kernel.Web.Mvc
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ApiController(CommandDispatcher commandDispatcher, QueryDispatcher queryDispatcher)
        {
            CommandDispatcher = commandDispatcher;
            QueryDispatcher = queryDispatcher;
        }

        protected CommandDispatcher CommandDispatcher { get; init; }

        protected QueryDispatcher QueryDispatcher { get; init; }
    }
}
