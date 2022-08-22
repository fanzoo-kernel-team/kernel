using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Commands.Games;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services;
using Fanzoo.Kernel.Web.Services.Configuration;
using Microsoft.AspNetCore.Mvc;

await using var application =
    WebApplication.CreateBuilder(args)
        .AddRESTApiCore<RESTApiUserAuthenticationService, User, UserIdentifierValue, Guid, EmailUsernameValue, PasswordValue, RefreshToken, RefreshTokenIdentifierValue, Guid>()
        .AddNHibernateCoreFromAssembly(Assembly.GetExecutingAssembly())
        .AddFrameworkCoreFromAssemblies(addTypes =>
            addTypes
                .FromAssembly(Assembly.GetExecutingAssembly()))
        .AddSetting<JwtSecurityTokenSettings>("Jwt")
        .AddFluentMigratorCoreFromAssembly(Assembly.GetExecutingAssembly())
            .Build();

application.MapGet("/heartbeat", () => { });

application.MapPost("/games", async ([FromQuery] string name, CommandDispatcher commandDispatcher) =>
{
    var result = await commandDispatcher.DispatchAsync(new CreateCommand(name));

    return result.IsSuccessful ? Results.Ok() : Results.BadRequest();
});

application
    .UseRESTApi()
        .RunWithMigrations();

public partial class Program { }
