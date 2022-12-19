using Fanzoo.Kernel.Web.Middleware;
using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;

namespace Fanzoo.Kernel.Builder
{
    public static class WebApplicationExtensions
    {
        private static WebApplication UseWebCore(this WebApplication application)
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

            return application;
        }

        public static WebApplication UseSwagger(this WebApplication application)
        {
            SwaggerBuilderExtensions.UseSwagger(application);
            application.UseSwaggerUI();

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

            migrationRunner?.MigrateUp();

            application.Run();
        }

        public static WebApplication UseApplicationModuleEndpoints(this WebApplication application)
        {
            foreach (var module in application.Services.GetServices<IApplicationModule>())
            {
                module.MapEndpoints(application);
            }

            return application;
        }
    }
}
