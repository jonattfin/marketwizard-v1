using Bogus;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace Infrastructure.Services;

public class MockMarketDataProvider : IMarketDataProvider
{
    private static readonly string[] CountryCodes =
    [
        "AUS",
        "CHN",
        "DEU",
        "ESP",
        "FRA",
        "GBR",
        "IND",
        "JPN",
        "KOR",
        "USA"
    ];

    public Task<IndicePerformanceData> GetIndicesAsync(CancellationToken cancellationToken = default)
    {
        var indices = CountryCodes.Select(code => new IndicePerformance
        {
            Name = code,
            CountryCode = code,
            RegularMarketChangePercent = Random.Shared.NextDouble() * Random.Shared.Next(-1, 2),
            RegularMarketPrice = Random.Shared.Next(5_000, 100_000)
        }).ToList();

        var data = new IndicePerformanceData { Items = indices, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<TopNewsData> GetTopNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsFaker = new Faker<TopNews>()
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(o => o.Text, f => f.Lorem.Sentence())
            .RuleFor(o => o.Description, f => f.Lorem.Sentence())
            .RuleFor(o => o.Url, f => f.Internet.Url())
            .RuleFor(o => o.Source, f => f.Company.CompanyName())
            .RuleFor(o => o.Country, f => f.PickRandom(CountryCodes))
            .RuleFor(o => o.Date, _ => DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .RuleFor(o => o.Sentiment, f => f.Random.Double(-1.0, 1.0));

        var data = new TopNewsData { Items = newsFaker.Generate(10), Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<SectorPerformanceData> GetSectorPerformanceAsync(CancellationToken cancellationToken = default)
    {
        var items = Enum.GetValues<SectorType>().Select(type => new SectorPerformance
        {
            Type = type.ToString(),
            Change = new Faker().Random.Double(-0.5, 0.5),
            Country = "USA"
        }).ToList();

        var data = new SectorPerformanceData { Items = items, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<GainersData> GetTopGainersAsync(CancellationToken cancellationToken = default)
    {
        var items = GetGainers(6, positiveOnly: true);
        var data = new GainersData { Items = items, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<GainersData> GetTopLosersAsync(CancellationToken cancellationToken = default)
    {
        var items = GetGainers(6, positiveOnly: false);
        var data = new GainersData { Items = items, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<GainersData> GetTopIndustriesAsync(CancellationToken cancellationToken = default)
    {
        var items = GetGainers(6, positiveOnly: true);
        var data = new GainersData { Items = items, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    public Task<GainersData> GetWorstIndustriesAsync(CancellationToken cancellationToken = default)
    {
        var items = GetGainers(6, positiveOnly: false);
        var data = new GainersData { Items = items, Date = DateTimeOffset.UtcNow };
        return Task.FromResult(data);
    }

    private static List<Gainers> GetGainers(int take = 12, bool? positiveOnly = null)
    {
        var faker = new Faker();
        var sectors = Enum.GetValues<SectorType>();

        var values = sectors.Select(type =>
        {
            var change = positiveOnly switch
            {
                true => faker.Random.Double(0.01, 0.5),
                false => faker.Random.Double(-0.5, -0.01),
                null => faker.Random.Double(-0.5, 0.5)
            };

            return new Gainers
            {
                Type = type.ToString(),
                Change = change,
                Country = "USA"
            };
        }).ToList();

        return values.Take(take).ToList();
    }
}
