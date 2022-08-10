namespace Fanzoo.Kernel.Web.Middleware
{
    public abstract class AspNetMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        protected AspNetMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await OnInvokeAsync(context);

            await _nextDelegate(context);
        }

        protected abstract Task OnInvokeAsync(HttpContext context);
    }
}
