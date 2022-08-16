using System.Net;

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
            catch (ResultFailureException<Error> e)
            {
                await HandleResultExceptionAsync(context, e);
            }
            catch
            {
                await HandleExceptionAsync(context);
            }
        }

        private static async ValueTask HandleResultExceptionAsync(HttpContext context, ResultFailureException<Error> e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(e.Error);
        }

        private static async ValueTask HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Internal Server Error");
        }
    }
}
