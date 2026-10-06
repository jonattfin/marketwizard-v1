using MarketWizard.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<WatchlistRepository>();
builder.Services.AddSingleton<MiscRepository>();

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

api.MapGet("indices", async (MiscRepository repo) => await repo.GetIndices()).WithName("GetIndices");
api.MapGet("top-news", async (MiscRepository repo) => await repo.GetTopNews()).WithName("GetTopNews");
api.MapGet("sector-performance", async (MiscRepository repo) => await repo.GetSectorPerformance()).WithName("GetSectorPerformance");

api.MapGet("top-gainers", async (MiscRepository repo) => await repo.GetTopGainers()).WithName("GetTopGainers");
api.MapGet("top-losers", async (MiscRepository repo) => await repo.GetTopLosers()).WithName("GetTopLosers");

api.MapGet("top-industries", async (MiscRepository repo) => await repo.GetTopIndustries()).WithName("GetTopIndustries");
api.MapGet("worst-industries", async (MiscRepository repo) => await repo.GetWorstIndustries()).WithName("GetWorstIndustries");

api.MapGet("watchlist", async (WatchlistRepository repo) => await repo.GetAll()).WithName("GetWatchlist");
api.MapPost("watchlist", async (WatchlistRepository repo) => await repo.CreateWatchlist()).WithName("CreateWatchlist");
api.MapPut("watchlist/{id}", async (string id, UpdateWatchlistDto dto, WatchlistRepository repo) =>
{
    var updated = await repo.UpdateWatchlist(id, dto);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
}).WithName("UpdateWatchlist");
api.MapDelete("watchlist/{id}", async (string id, WatchlistRepository repo) =>
{
    await repo.DeleteWatchlist(id);
    return Results.NoContent();
}).WithName("DeleteWatchlist");

// api.MapPost("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.CreateWatchlistItem(dto)).WithName("CreateWatchlistItem");
// api.MapDelete("watchlist-item", async (WatchlistItemDto dto, WatchlistRepository repo) => await repo.DeleteWatchlistItem(dto)).WithName("DeleteWatchlistItem");

app.MapDefaultEndpoints();

app.UseFileServer();

await app.RunAsync();