using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fanzoo.Kernel.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RedirectToReturnUrlAttribute : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (context.Result is RedirectToPageResult)
            {
                var returnUrl = context.HttpContext.Request.Query["ReturnUrl"].ToString();

                if (returnUrl.IsNotNullOrWhitespace())
                {
                    context.Result = new RedirectResult(returnUrl);
                }
            }
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // Method intentionally left empty.
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // Method intentionally left empty.
        }
    }
}
