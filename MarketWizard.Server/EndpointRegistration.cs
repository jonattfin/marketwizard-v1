using MarketWizard.Application.Services;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Server;

public static class EndpointRegistration
{
    public static void MapEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapGet("indices", async (IMiscService service, CancellationToken cancellationToken) => await service.GetIndices(cancellationToken))
            .WithName("GetIndices");
        api.MapGet("top-news", async (IMiscService service, CancellationToken cancellationToken) => await service.GetTopNews(cancellationToken))
            .WithName("GetTopNews");
        api.MapGet("sector-performance", async (IMiscService service, CancellationToken cancellationToken) => await service.GetSectorPerformance(cancellationToken))
            .WithName("GetSectorPerformance");

        api.MapGet("top-gainers", async (IMiscService service, CancellationToken cancellationToken) => await service.GetTopGainers(cancellationToken))
            .WithName("GetTopGainers");
        api.MapGet("top-losers", async (IMiscService service, CancellationToken cancellationToken) => await service.GetTopLosers(cancellationToken))
            .WithName("GetTopLosers");

        api.MapGet("top-industries", async (IMiscService service, CancellationToken cancellationToken) => await service.GetTopIndustries(cancellationToken))
            .WithName("GetTopIndustries");
        api.MapGet("worst-industries", async (IMiscService service, CancellationToken cancellationToken) => await service.GetWorstIndustries(cancellationToken))
            .WithName("GetWorstIndustries");

        api.MapGet("watchlist", async (IWatchlistService service, CancellationToken cancellationToken) => await service.GetAll(cancellationToken))
            .WithName("GetWatchlist");
        api.MapPost("watchlist", async (CreateWatchlistDto? dto, IWatchlistService service, CancellationToken cancellationToken) =>
        {
            var created = await service.CreateWatchlist(dto, cancellationToken);
            return Results.Created($"/api/watchlist/{created.Id}", created);
        }).WithName("CreateWatchlist");
        api.MapPut("watchlist/{id}", async (string id, UpdateWatchlistDto dto, IWatchlistService service, CancellationToken cancellationToken) =>
        {
            var updated = await service.UpdateWatchlist(id, dto, cancellationToken);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        }).WithName("UpdateWatchlist");
        api.MapDelete("watchlist/{id}", async (string id, IWatchlistService service, CancellationToken cancellationToken) =>
        {
            await service.DeleteWatchlist(id, cancellationToken);
            return Results.NoContent();
        }).WithName("DeleteWatchlist");
        
        // api.MapPost("watchlist-item", async (WatchlistItemDto dto, IWatchlistService service, CancellationToken cancellationToken) =>
        // {
        //     await service.CreateWatchlistItem(dto, cancellationToken);
        //     return Results.Ok();
        // }).WithName("CreateWatchlistItem");
        //
        // api.MapDelete("watchlist-item", async (WatchlistItemDto dto, IWatchlistService service, CancellationToken cancellationToken) =>
        // {
        //     var deleted = await service.DeleteWatchlistItem(dto, cancellationToken);
        //     return deleted ? Results.NoContent() : Results.NotFound();
        // }).WithName("DeleteWatchlistItem");

    }
}