using System.Web;

namespace Fanzoo.Kernel.Web.Middleware
{
    public sealed class RazorPagesForcePasswordChangeMiddleware : AspNetMiddleware
    {
        private readonly RazorPagesForcePasswordChangeMiddlewareOptions _options;

        public RazorPagesForcePasswordChangeMiddleware(RequestDelegate nextDelegate, RazorPagesForcePasswordChangeMiddlewareOptions options) : base(nextDelegate)
        {
            _options = options;
        }

        protected override Task OnInvokeAsync(HttpContext context)
        {
            if (!_options.IgnoredRoutes.Select(r => new PathString(r)).Contains(context.Request.Path))
            {
                var claim = context.User.GetClaimOrDefault(ClaimTypes.ForcePasswordChange);

                if (claim is not null)
                {
                    var returnUrl = context.Request.Path.Value == "/" ? "" : "?returnUrl=" + HttpUtility.UrlEncode(context.Request.Path.Value);
                    context.Response.Redirect(_options.ChangePasswordRoute + returnUrl);
                }
            }

            return Task.CompletedTask;
        }
    }

    public sealed class RazorPagesForcePasswordChangeMiddlewareOptions
    {
        public RazorPagesForcePasswordChangeMiddlewareOptions(string changePasswordRoute, params string[] routesToIgnore)
        {
            ChangePasswordRoute = changePasswordRoute;

            var routes = new List<string>(routesToIgnore);

            if (!routes.Contains(changePasswordRoute))
            {
                routes.Add(changePasswordRoute);
            }

            IgnoredRoutes = routes.ToArray();
        }

        public IEnumerable<string> IgnoredRoutes { get; init; }

        public string ChangePasswordRoute { get; init; }
    }
}
