using Bogus;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace Infrastructure.Repositories;

public class MiscRepository : IMiscRepository
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

        var data = new IndicePerformanceData() { Items = indices};
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

        var data = new TopNewsData() { Items = news.Generate(10) };
        return await Task.FromResult(data);
    }

    public async Task<SectorPerformanceData> GetSectorPerformance()
    {
        var items = Enum.GetValues<SectorType>().Select(type => new SectorPerformance()
        {
            Type = type.ToString(),
            Change = new Faker().Random.Double(-0.5, 0.5)
        }).ToList();

        var data = new SectorPerformanceData() { Items = items };
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopGainers()
    {
        var items = GetGainers(6);
        var data = new GainersData{ Items = items };
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopLosers()
    {
        var items = GetGainers(6);
        var data = new GainersData{ Items = items };
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetTopIndustries()
    {
        var items = GetGainers(6);
        var data = new GainersData{ Items = items };
        return await Task.FromResult(data);
    }

    public async Task<GainersData> GetWorstIndustries()
    {
        var items = GetGainers(6);
        var data = new GainersData{ Items = items };
        return await Task.FromResult(data);
    }

    private List<Gainers> GetGainers(int take = 12)
    {
        Gainers[] values =
        [
            .. Enum.GetValues<SectorType>().Select(type => new Gainers()
            {
                Type = type.ToString(),
                Change = new Faker().Random.Double(-0.5, 0.5)
            })
        ];

        return [.. values.Take(take)];
    }
}