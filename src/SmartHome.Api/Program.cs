using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
    
    // Dev only db check endpoint
    app.MapGet("/health/db", async (AppDbContext db) =>
	    await db.Database.CanConnectAsync()
		    ? Results.Ok("Database reachable")
		    : Results.Problem("Database not reachable"));
}

app.MapGet("/", () => "Hello World!");

app.Run();