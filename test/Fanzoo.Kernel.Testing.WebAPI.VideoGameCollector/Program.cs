using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Defaults.Builder;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services;
using Fanzoo.Kernel.Web.Services.Configuration;

var isStandAlone = Environment.GetEnvironmentVariable("RUN_MODE") == "Stand-alone";

if (isStandAlone)
{
    Console.WriteLine("Spinning up temp database...");

    //temp db stuff
    LocalDbHelper.StartUpInstance("kernel-test-instance", "videogames");
    LocalDbHelper.CreateDatabase("kernel-test-instance", "videogames");

    Console.WriteLine("Done.");

}

var builder =
    WebApplication.CreateBuilder(args)
        .AddRESTApiCore<RESTApiUserAuthenticationService>()
        .AddSwagger()
        .AddApplicationModulesFromAssembly(Assembly.GetExecutingAssembly())
        .AddNHibernateCoreFromAssembly(Assembly.GetExecutingAssembly())
        .AddFrameworkCoreFromAssemblies(addTypes =>
            addTypes
                .FromAssembly(Assembly.GetExecutingAssembly()))
        .AddSetting<JwtSecurityTokenSettings>("Jwt")
        .AddFluentMigratorCoreFromAssembly(Assembly.GetExecutingAssembly());

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
}

if (isStandAlone)
{
    //clean up db stuff when the app stops
    application.Lifetime.ApplicationStopping.Register(() =>
    {
        Console.WriteLine("Shutting down temp database...");

        LocalDbHelper.CleanUpInstance("kernel-instance");

        Console.WriteLine("Done.");

    });

}

application
    .UseRESTApi()
    .UseApplicationModuleEndpoints()
        .RunWithMigrations();

public partial class Program { }
