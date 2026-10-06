using Infrastructure.Repositories;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddSingleton<IMiscRepository, MiscRepository>();

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

api.MapGet("indices", async (IMiscRepository repo) => await repo.GetIndices()).WithName("GetIndices");
api.MapGet("top-news", async (IMiscRepository repo) => await repo.GetTopNews()).WithName("GetTopNews");
api.MapGet("sector-performance", async (IMiscRepository repo) => await repo.GetSectorPerformance()).WithName("GetSectorPerformance");

api.MapGet("top-gainers", async (IMiscRepository repo) => await repo.GetTopGainers()).WithName("GetTopGainers");
api.MapGet("top-losers", async (IMiscRepository repo) => await repo.GetTopLosers()).WithName("GetTopLosers");

api.MapGet("top-industries", async (IMiscRepository repo) => await repo.GetTopIndustries()).WithName("GetTopIndustries");
api.MapGet("worst-industries", async (IMiscRepository repo) => await repo.GetWorstIndustries()).WithName("GetWorstIndustries");

api.MapGet("watchlist", async (IWatchlistRepository repo) => await repo.GetAll()).WithName("GetWatchlist");
api.MapPost("watchlist", async (IWatchlistRepository repo) => await repo.CreateWatchlist()).WithName("CreateWatchlist");
api.MapPut("watchlist/{id}", async (string id, UpdateWatchlistDto dto, IWatchlistRepository repo) =>
{
    var updated = await repo.UpdateWatchlist(id, dto);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
}).WithName("UpdateWatchlist");
api.MapDelete("watchlist/{id}", async (string id, IWatchlistRepository repo) =>
{
    await repo.DeleteWatchlist(id);
    return Results.NoContent();
}).WithName("DeleteWatchlist");

// api.MapPost("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.CreateWatchlistItem(dto)).WithName("CreateWatchlistItem");
// api.MapDelete("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.DeleteWatchlistItem(dto)).WithName("DeleteWatchlistItem");

app.MapDefaultEndpoints();

app.UseFileServer();

await app.RunAsync();