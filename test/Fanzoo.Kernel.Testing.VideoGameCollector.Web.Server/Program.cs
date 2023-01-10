using Fanzoo.Kernel.Testing.VideoGameCollector.Web.Server.Data;
using Fanzoo.Kernel.Testing.VideoGameCollector.Web.Server.Modules.Session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5043") });

builder.Services.AddSingleton<WeatherForecastService>();

//TODO: when this is added to kernel it will be scanned and registered automatically
var sessionHost = new SessionClientHost();

sessionHost.ConfigureServices(builder.Services);

builder.Services.AddSingleton <
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
