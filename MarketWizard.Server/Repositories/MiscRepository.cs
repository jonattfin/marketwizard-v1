using Bogus;

namespace MarketWizard.Server.Repositories;

public class MiscRepository
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

    public async Task<IndicePerformanceData> GetIndices()
    {
        var indices = CountryCodes.Select(code => new IndicePerformance
        {
            Name = code,
            CountryCode = code,
            RegularMarketChangePercent = Random.Shared.NextDouble() * Random.Shared.Next(-1, 2),
            RegularMarketPrice = Random.Shared.Next(5_000, 100_000)
        }).ToList();

        var data = new IndicePerformanceData(indices, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<TopNewsData> GetTopNews()
    {
        var news = new Faker<TopNews>()
            .RuleFor(o => o.Text, f => f.Lorem.Sentence())
            .RuleFor(o => o.Description, f => f.Lorem.Sentence())
            .RuleFor(o => o.Url, f => f.Internet.Url())
            .RuleFor(o => o.Source, f => f.Internet.Url())
            .RuleFor(o => o.Source, f => f.Company.CompanyName());

        var data = new TopNewsData(news.Generate(10), new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<SectorPerformanceData> GetSectorPerformance()
    {
        var sectorPerformance = Enum.GetValues<SectorType>().Select(type => new SectorPerformance()
        {
            Type = type.ToString(),
            Change = new Faker().Random.Double(-0.5, 0.5)
        }).ToList();

        var data = new SectorPerformanceData(sectorPerformance, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopGainers()
    {
        var topGainers = GetGainers(6);
        var data = new GainersData(topGainers, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopLosers()
    {
        var topLosers = GetGainers(6);
        var data = new GainersData(topLosers, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopIndustries()
    {
        var topIndustries = GetGainers(6);
        var data = new GainersData(topIndustries, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetWorstIndustries()
    {
        var worstIndustries = GetGainers(6);
        var data = new GainersData(worstIndustries, new DateTimeOffset(DateTime.Now));
        return await Task.FromResult(data);
    }
    
    private List<Gainers> GetGainers(int take = 12)
    {
        Gainers[] values = [
            .. Enum.GetValues<SectorType>().Select(type => new Gainers()
            {
                Type = type.ToString(),
                Change = new Faker().Random.Double(-0.5, 0.5)
            })
        ];
        
        return [.. values.Take(take)];
    }
}