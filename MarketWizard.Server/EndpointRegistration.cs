using MarketWizard.Application.Features.Misc.Queries;
using MarketWizard.Application.Features.Watchlists.Commands;
using MarketWizard.Application.Features.Watchlists.Queries;
using MarketWizard.Domain.Entities;
using MediatR;

namespace MarketWizard.Server;

public static class EndpointRegistration
{
    public static void MapEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapGet("indices", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetIndicesQuery(), cancellationToken))
            .WithName("GetIndices");
        api.MapGet("top-news", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetTopNewsQuery(), cancellationToken))
            .WithName("GetTopNews");
        api.MapGet("sector-performance", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetSectorPerformanceQuery(), cancellationToken))
            .WithName("GetSectorPerformance");

        api.MapGet("top-gainers", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetTopGainersQuery(), cancellationToken))
            .WithName("GetTopGainers");
        api.MapGet("top-losers", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetTopLosersQuery(), cancellationToken))
            .WithName("GetTopLosers");

        api.MapGet("top-industries", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetTopIndustriesQuery(), cancellationToken))
            .WithName("GetTopIndustries");
        api.MapGet("worst-industries", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetWorstIndustriesQuery(), cancellationToken))
            .WithName("GetWorstIndustries");

        api.MapGet("watchlist",
                async (int? pageNumber, int? pageSize, ISender sender, CancellationToken cancellationToken) =>
                    await sender.Send(new GetWatchlistsQuery(pageNumber ?? 1, pageSize ?? 10), cancellationToken))
            .WithName("GetWatchlist");
        api.MapPost("watchlist", async (CreateWatchlistDto? dto, ISender sender, CancellationToken cancellationToken) =>
        {
            var created = await sender.Send(new CreateWatchlistCommand(dto), cancellationToken);
            return Results.Created($"/api/watchlist/{created.Id}", created);
        }).WithName("CreateWatchlist");
        api.MapPut("watchlist/{id}",
            async (string id, UpdateWatchlistDto dto, ISender sender, CancellationToken cancellationToken) =>
            {
                var updated = await sender.Send(new UpdateWatchlistCommand(id, dto), cancellationToken);
                return updated is not null ? Results.Ok(updated) : Results.NotFound();
            }).WithName("UpdateWatchlist");
        api.MapDelete("watchlist/{id}", async (string id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteWatchlistCommand(id), cancellationToken);
            return Results.NoContent();
        }).WithName("DeleteWatchlist");

        // api.MapPost("watchlist-item", async (WatchlistItemDto dto, ISender sender, CancellationToken cancellationToken) =>
        // {
        //     await sender.Send(new CreateWatchlistItemCommand(dto), cancellationToken);
        //     return Results.Ok();
        // }).WithName("CreateWatchlistItem");
        //
        // api.MapDelete("watchlist-item", async (WatchlistItemDto dto, ISender sender, CancellationToken cancellationToken) =>
        // {
        //     var deleted = await sender.Send(new DeleteWatchlistItemCommand(dto), cancellationToken);
        //     return deleted ? Results.NoContent() : Results.NotFound();
        // }).WithName("DeleteWatchlistItem");
    }
}