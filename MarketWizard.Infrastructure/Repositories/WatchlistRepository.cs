using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace Infrastructure.Repositories;

public class WatchlistRepository : IWatchlistRepository
{
    private readonly List<WatchlistType> _watchlists =
    [
        .. Enumerable.Range(1, 5).Select((_) => new WatchlistType()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Watchlist " + Guid.NewGuid(),
            Items =
            [
                new WatchlistItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "WatchlistItem " + Guid.NewGuid(),
                    Description = "Description",
                    Ticker = "Ticker",
                }
            ]
        })
    ];

    public Task<List<WatchlistType>> GetAll(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_watchlists);
    }

    public Task<WatchlistType> CreateWatchlist(CreateWatchlistDto? dto = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var name = string.IsNullOrWhiteSpace(dto?.Name) ? "Watchlist " + Guid.NewGuid() : dto.Name;
        var watchlist = new WatchlistType
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Items = []
        };
        _watchlists.Add(watchlist);
        return Task.FromResult(watchlist);
    }

    public Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == id);
        watchlist?.Name = dto.Name;

        return Task.FromResult(watchlist);
    }

    public Task<bool> DeleteWatchlist(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var removed = _watchlists.RemoveAll(w => w.Id == id);
        return Task.FromResult(removed > 0);
    }

    public Task CreateWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var targetWatchlistId = dto.WatchlistId ?? dto.Id;
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == targetWatchlistId);
        var ticker = dto.Ticker ?? string.Empty;
        watchlist?.Items.Add(new WatchlistItemType
        {
            Id = Guid.NewGuid().ToString(),
            Name = ticker,
            Description = "Description",
            Ticker = ticker
        });
        
        return Task.CompletedTask;
    }

    public Task<bool> DeleteWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var targetWatchlistId = dto.WatchlistId ?? dto.Id;
        var targetItemOrTicker = dto.ItemId ?? dto.Ticker;
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == targetWatchlistId);
        var removed = watchlist?.Items.RemoveAll(i => i.Ticker == targetItemOrTicker || i.Id == targetItemOrTicker);
        
        return Task.FromResult((removed ?? 0) > 0);
    }
}

