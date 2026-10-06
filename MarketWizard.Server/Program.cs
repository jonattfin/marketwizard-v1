using Infrastructure.Repositories;
using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using MarketWizard.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddSingleton<IMiscRepository, MiscRepository>();
builder.Services.AddSingleton<IMiscService, MiscService>();
builder.Services.AddSingleton<IWatchlistService, WatchlistService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var api = app.MapGroup("/api");

api.MapGet("indices", async (IMiscService service) => await service.GetIndices()).WithName("GetIndices");
api.MapGet("top-news", async (IMiscService service) => await service.GetTopNews()).WithName("GetTopNews");
api.MapGet("sector-performance", async (IMiscService service) => await service.GetSectorPerformance()).WithName("GetSectorPerformance");

api.MapGet("top-gainers", async (IMiscService service) => await service.GetTopGainers()).WithName("GetTopGainers");
api.MapGet("top-losers", async (IMiscService service) => await service.GetTopLosers()).WithName("GetTopLosers");

api.MapGet("top-industries", async (IMiscService service) => await service.GetTopIndustries()).WithName("GetTopIndustries");
api.MapGet("worst-industries", async (IMiscService service) => await service.GetWorstIndustries()).WithName("GetWorstIndustries");

api.MapGet("watchlist", async (IWatchlistService service) => await service.GetAll()).WithName("GetWatchlist");
api.MapPost("watchlist", async (IWatchlistService service) => await service.CreateWatchlist()).WithName("CreateWatchlist");
api.MapPut("watchlist/{id}", async (string id, UpdateWatchlistDto dto, IWatchlistService service) =>
{
    var updated = await service.UpdateWatchlist(id, dto);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
}).WithName("UpdateWatchlist");
api.MapDelete("watchlist/{id}", async (string id, IWatchlistService service) =>
{
    await service.DeleteWatchlist(id);
    return Results.NoContent();
}).WithName("DeleteWatchlist");

// api.MapPost("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.CreateWatchlistItem(dto)).WithName("CreateWatchlistItem");
// api.MapDelete("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.DeleteWatchlistItem(dto)).WithName("DeleteWatchlistItem");

app.MapDefaultEndpoints();

app.UseFileServer();

await app.RunAsync();