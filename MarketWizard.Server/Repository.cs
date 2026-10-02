using Bogus;

namespace MarketWizard.Server;

public class Repository
{
    public WeatherForecast[] GetWeatherForecast()
    {
        string[] summaries =
            ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];


        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    }

    public IndicePerformanceData GetIndices()
    {
        var indices = new Faker<IndicePerformance>()
            .RuleFor(o => o.Name, f => f.Finance.Account())
            .RuleFor(o => o.RegularMarketChangePercent, f => f.Random.Double(-100, 100))
            .RuleFor(o => o.RegularMarketPrice, f => f.Random.Double(-100, 100));
        
        return new IndicePerformanceData(indices.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public TopNewsData GetTopNews()
    {
        var news = new Faker<TopNews>()
            .RuleFor(o => o.Text, f => f.Lorem.Sentence())
            .RuleFor(o => o.Description, f => f.Lorem.Sentence())
            .RuleFor(o => o.Url, f => f.Internet.Url())
            .RuleFor(o => o.Source, f => f.Internet.Url())
            .RuleFor(o => o.Source, f => f.Company.CompanyName());

        return new TopNewsData(news.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public SectorPerformanceData GetSectorPerformance()
    {
        var sectorPerformance = new Faker<SectorPerformance>();
        return new SectorPerformanceData(sectorPerformance.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public GainersData GetTopGainers()
    {
        var gainers = new Faker<Gainers>()
            .RuleFor(o => o.Country, f => f.Company.CompanyName())
            .RuleFor(o => o.Change, f => f.Random.Double(-100, 100))
            .RuleFor(o => o.Type, f => SectorType.ConsumerGoods);
        
        return new GainersData(gainers.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public GainersData GetTopLosers()
    {
        var gainers = new Faker<Gainers>()
            .RuleFor(o => o.Country, f => f.Company.CompanyName())
            .RuleFor(o => o.Change, f => f.Random.Double(-100, 100))
            .RuleFor(o => o.Type, f => SectorType.ConsumerGoods);
        
        return new GainersData(gainers.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public GainersData GetTopIndustries()
    {
        var gainers = new Faker<Gainers>()
            .RuleFor(o => o.Country, f => f.Company.CompanyName())
            .RuleFor(o => o.Change, f => f.Random.Double(-100, 100))
            .RuleFor(o => o.Type, f => SectorType.ConsumerGoods);

        return new GainersData(gainers.Generate(5), new DateTimeOffset(DateTime.Now));
    }

    public GainersData GetWorstIndustries()
    {
        var gainers = new Faker<Gainers>()
            .RuleFor(o => o.Country, f => f.Company.CompanyName())
            .RuleFor(o => o.Change, f => f.Random.Double(-100, 100))
            .RuleFor(o => o.Type, f => SectorType.ConsumerGoods);
        
        return new GainersData(gainers.Generate(5), new DateTimeOffset(DateTime.Now));
    }
}