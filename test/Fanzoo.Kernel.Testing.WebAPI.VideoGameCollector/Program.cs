using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities;
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
        .AddRESTApiCore<RESTApiUserAuthenticationService, User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>()
        .AddApplicationModulesFromAssembly(Assembly.GetExecutingAssembly())
        .AddNHibernateCoreFromAssembly(Assembly.GetExecutingAssembly())
        .AddFrameworkCoreFromAssemblies(addTypes =>
            addTypes
                .FromAssembly(Assembly.GetExecutingAssembly()))
        .AddSetting<JwtSecurityTokenSettings>("Jwt")
        .AddFluentMigratorCoreFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var application = builder.Build();

application.MapGet("/heartbeat", () => { });

if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
    application.UseSwaggerUI();
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
