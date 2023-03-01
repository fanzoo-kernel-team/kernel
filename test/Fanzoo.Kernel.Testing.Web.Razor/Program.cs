using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Defaults.Builder;

var isStandAlone = Environment.GetEnvironmentVariable("RUN_MODE") == "Stand-alone";

if (isStandAlone)
{
    Console.WriteLine("Spinning up temp database...");

    //temp db stuff
    LocalDbHelper.StartUpInstance("kernel-test-instance", "videogames");
    LocalDbHelper.CreateDatabase("kernel-test-instance", "videogames");

    Console.WriteLine("Done.");

}

var application = WebApplication.CreateBuilder(args)
    .AddRazorPagesCore<RazorPagesUserAuthenticationService>()
    .AddNHibernateCoreFromAssembly(Assembly.GetExecutingAssembly())
    .AddFrameworkCoreFromAssemblies(addTypes =>
        addTypes
            .FromAssembly(Assembly.GetExecutingAssembly()))
    .AddFluentMigratorCoreFromAssembly(Assembly.GetExecutingAssembly())
    .AddLogging()
        .Build();

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
    .UseRazorPagesCore()
    .UseRazorPagesForcePasswordChangeMiddleware(new("/account/changepassword", "/account/logout"))
    .RunWithMigrations();

