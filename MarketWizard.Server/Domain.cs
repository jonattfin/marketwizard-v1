public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record IndicePerformance(
    int Id,
    string Name,
    string CountryCode,
    double RegularMarketChangePercent,
    double RegularMarketPrice
)
{
    public IndicePerformance() : this(default!, default!, default!, default!, default!)
    {
    }
}

public record IndicePerformanceData(
    IReadOnlyList<IndicePerformance> Items,
    DateTimeOffset? Date = null
);

public record TopNews(
    int Id,
    string Text,
    long? Date = null,
    double? Sentiment = null,
    string? Country = null,
    string? Source = null,
    string? Description = null,
    string? Url = null
)
{
    public TopNews() : this(default!, default!)
    {
    }
}

public record TopNewsData(
    IReadOnlyList<TopNews> Items,
    DateTimeOffset? Date = null
);

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

public record SectorPerformance(
    SectorType Type,
    double Change,
    string Country
)
{
    public SectorPerformance() : this(default!, default!, default!)
    {
        
    }
}

public record SectorPerformanceData(
    IReadOnlyList<SectorPerformance> Items,
    DateTimeOffset? Date = null
);

public record Gainers(
    SectorType Type,
    double Change,
    string Country
)
{
    public Gainers() : this(default!, default!, default!)
    {
    }
}

public record GainersData(
    IReadOnlyList<Gainers> Items,
    DateTimeOffset? Date = null
);