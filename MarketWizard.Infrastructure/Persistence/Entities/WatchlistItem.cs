namespace Infrastructure.Persistence.Entities;

public class WatchlistItem
{
    public Guid Id { get; set; }
    public Guid WatchlistId { get; set; }
    public Watchlist? Watchlist { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
}
