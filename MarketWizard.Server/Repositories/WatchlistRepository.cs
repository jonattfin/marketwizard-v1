namespace MarketWizard.Server.Repositories;

public class WatchlistRepository
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

    public Task UpdateWatchlist(string id, string name)
    {
        var watchlist = _watchlists.FirstOrDefault(w => w.Id == id);
        if (watchlist != null)
        {
            watchlist.Name = name;
        }

        return Task.CompletedTask;
    }

    public Task DeleteWatchlist(string id)
    {
        _watchlists.RemoveAll(w => w.Id == id);
        return Task.CompletedTask;
    }
}

public class WatchlistItemType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Ticker { get; set; }
}

public class WatchlistType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<WatchlistItemType> Items { get; set; }
}