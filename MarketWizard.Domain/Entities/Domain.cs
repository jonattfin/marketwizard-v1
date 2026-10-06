namespace MarketWizard.Domain.Entities;

public class IndicePerformance
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public double RegularMarketChangePercent { get; set; }
    public double RegularMarketPrice { get; set; }
}

public class IndicePerformanceData : GenericData<IndicePerformance>
{
}

public class TopNews
{
    public int Id { get; set; }
    public string Text { get; set; }
    public long? Date { get; set; }
    public double? Sentiment { get; set; }
    public string? Country { get; set; }
    public string? Source { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
}

public class TopNewsData : GenericData<TopNews>
{
}

public enum SectorType
{
    BasicMaterials,
    Telecom,
    ConsumerGoods,
    CustomerStaples,
    ConsumerServices,
    Energy,
    Financials,
    HealthCare,
    Industrials,
    Materials,
    Utilities,
    Technology
}

public class SectorPerformance
{
    public string Type { get; set; }
    public double Change { get; set; }
    public string Country { get; set; }
}

public class SectorPerformanceData : GenericData<SectorPerformance>
{
}

public class Gainers
{
    public string Type { get; set; }
    public double Change { get; set; }
    public string Country { get; set; }
}

public class GainersData : GenericData<Gainers>
{
}

public class GenericData<T>
{
    public IReadOnlyList<T> Items { get; set; }
    public DateTimeOffset? Date { get; set; } = new DateTimeOffset(DateTime.Now);
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

public record UpdateWatchlistDto(string Name);
public record WatchlistItemDto(string Id, string Ticker);