namespace Infrastructure.Persistence.Entities;

public class Watchlist
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<WatchlistItem> Items { get; set; } = [];
}
