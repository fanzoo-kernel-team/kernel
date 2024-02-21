using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanzoo.Kernel.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RedirectAuthenticatedAttribute(string redirectUrl) : Attribute, IAsyncPageFilter
    {
        private string RedirectUrl { get; set; } = redirectUrl;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity is not null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult(RedirectUrl);

                return;
            }

            await next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
    }
}
