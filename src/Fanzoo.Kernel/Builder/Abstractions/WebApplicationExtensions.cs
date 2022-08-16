using Fanzoo.Kernel.Web.Middleware;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fanzoo.Kernel.Builder
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseWebCore(this WebApplication application)
        {
            application
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseCors(options =>
                {
                    options.SetIsOriginAllowed(origin => true);
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowCredentials();
                })
                .UseAuthentication()
                .UseAuthorization();

            return application;
        }

        public static WebApplication UseRazorPagesCore(this WebApplication application)
        {
            application.UseWebCore();

            application.MapRazorPages();

            application.MapDefaultControllerRoute();

            application.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Strict });

            if (!application.Environment.IsDevelopment())
            {
                application.UseExceptionHandler("/Error");

            }

            return application;
        }

        public static WebApplication UseRESTApi(this WebApplication application)
        {
            application.UseMiddleware<ExceptionHandlerMiddleware>();

            application.UseWebCore();

            application.MapControllers();

            return application;
        }

        public static WebApplication UseRazorPagesForcePasswordChangeMiddleware(this WebApplication application, RazorPagesForcePasswordChangeMiddlewareOptions options)
        {
            application.UseMiddleware<RazorPagesForcePasswordChangeMiddleware>(options);

            return application;
        }

        public static void RunWithMigrations(this WebApplication application)
        {
            using var scope = application.Services.CreateScope();

            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            if (migrationRunner is not null)
            {
                migrationRunner.MigrateUp();
            }

            application.Run();
        }
    }
}
