var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

var api = app.MapGroup("/api");
api.MapGet("weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

api.MapGet("indices", () =>
{
    return new IndicePerformanceData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetIndices");

api.MapGet("top-news", () =>
{
    return new TopNewsData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetTopNews");

api.MapGet("sector-performance", () =>
{
    return new SectorPerformanceData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetSectorPerformance");

api.MapGet("top-gainers", () =>
{
    return new GainersData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetTopGainers");

api.MapGet("top-losers", () =>
{
    return new GainersData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetTopLosers");

api.MapGet("top-industries", () =>
{
    return new GainersData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetTopIndustries");

api.MapGet("worst-industries", () =>
{
    return new GainersData([],  new DateTimeOffset(DateTime.Now));
})
.WithName("GetWorstIndustries");

app.MapDefaultEndpoints();

app.UseFileServer();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record IndicePerformance(
    int Id,
    string Name,
    string CountryCode,
    double RegularMarketChangePercent,
    double RegularMarketPrice
);

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
);

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
);

public record SectorPerformanceData(
    IReadOnlyList<SectorPerformance> Items,
    DateTimeOffset? Date = null
);

public record Gainers(
    SectorType Type,
    double Change,
    string Country
);

public record GainersData(
    IReadOnlyList<Gainers> Items,
    DateTimeOffset? Date = null
);