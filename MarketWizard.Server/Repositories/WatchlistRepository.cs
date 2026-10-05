namespace MarketWizard.Server.Repositories;

public class WatchlistRepository
{
    public Task<List<WatchlistType>> GetAll()
    {
        var data = Enumerable.Empty<WatchlistType>().ToList();
        return Task.FromResult(data);
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