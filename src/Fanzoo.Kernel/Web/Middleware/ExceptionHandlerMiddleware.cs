using System.Net;
using Serilog;

namespace Fanzoo.Kernel.Web.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public ExceptionHandlerMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextDelegate(context);
            }
            catch (KernelErrorException e)
            {
                Log.Logger.Error(e, "An unhandled Kernel Error in a 500 Internal Server Error was thrown:");

                await HandleResultExceptionAsync(context, e);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "An unhandled exception resulting in a 500 Internal Server Error was thrown:");

                await HandleExceptionAsync(context);
            }
        }

        private static async ValueTask HandleResultExceptionAsync(HttpContext context, KernelErrorException e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(e.Message);
        }

        private static async ValueTask HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Internal Server Error");
        }
    }
}
