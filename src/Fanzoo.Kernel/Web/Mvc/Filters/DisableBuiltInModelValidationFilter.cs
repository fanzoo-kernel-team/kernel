using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanzoo.Kernel.Web.Mvc.Filters
{
    public class DisableBuiltInModelValidationFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            context.ModelState.Clear();

            await next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
    }
}
