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

    public Task<List<WatchlistType>> GetAll()
    {
        return Task.FromResult(_watchlists);
    }

    public Task CreateWatchlist()
    {
        return Task.CompletedTask;
    }

    public Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto)
    {
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == id);
        watchlist?.Name = dto.Name;

        return Task.FromResult(watchlist);
    }

    public Task DeleteWatchlist(string id)
    {
        _watchlists.RemoveAll(w => w.Id == id);
        return Task.CompletedTask;
    }

    public Task CreateWatchlistItem(WatchlistItemDto dto)
    {
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == dto.Id);
        watchlist?.Items.Add(new WatchlistItemType
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Ticker,
            Description = "Description",
            Ticker = dto.Ticker
        });
        
        return Task.CompletedTask;
    }

    public Task DeleteWatchlistItem(WatchlistItemDto dto)
    {
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == dto.Id);
        watchlist?.Items.RemoveAll(i => i.Ticker == dto.Ticker);
        
        return Task.CompletedTask;
    }
}

