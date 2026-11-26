using Azure;

using Microsoft.EntityFrameworkCore;

using SmartHome.Api;
using SmartHome.Api.Clients;
using SmartHome.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("smarthome")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<UrbanApiClient>(c =>
{
	c.BaseAddress = new Uri("https://urban-sense-api.azure-api.net/urban-sense/");
	c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", builder.Configuration["UrbanSense:Key"]);
});

builder.Services.AddSingleton<UrbanApiPoller>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<UrbanApiPoller>());

var app = builder.Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    //scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
    
    // Dev only db check endpoint
    app.MapGet("/health/db", async (AppDbContext db) =>
	    await db.Database.CanConnectAsync()
		    ? Results.Ok("Database reachable")
		    : Results.Problem("Database not reachable"));
    
	app.MapGet("/test", (UrbanApiPoller poller) => poller.Latest);
}

app.MapGet("/", () => "Hello World!");


app.Run();