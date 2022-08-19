using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Web.Services;
using Fanzoo.Kernel.Web.Services.Configuration;

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

application
    .UseRESTApi()
        .RunWithMigrations();

public partial class Program { }
