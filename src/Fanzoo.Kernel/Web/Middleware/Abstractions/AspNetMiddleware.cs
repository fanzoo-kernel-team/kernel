namespace Fanzoo.Kernel.Web.Middleware
{
    public abstract class AspNetMiddleware(RequestDelegate nextDelegate)
    {
        private readonly RequestDelegate _nextDelegate = nextDelegate;

        public async Task InvokeAsync(HttpContext context)
        {
            await OnInvokeAsync(context);

            await _nextDelegate(context);
        }

        protected abstract Task OnInvokeAsync(HttpContext context);
    }
}
