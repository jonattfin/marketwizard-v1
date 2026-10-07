using Infrastructure;
using Infrastructure.Persistence;
using MarketWizard.Application;
using MarketWizard.Server;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddApplicationServices();
builder.AddInfrastructureServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MarketWizardContext>();
    await db.Database.MigrateAsync();
}

app.MapEndpoints();

app.MapDefaultEndpoints();

app.UseFileServer();

await app.RunAsync();