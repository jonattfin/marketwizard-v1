using System.Text.Json;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using Infrastructure.Repositories;
using MarketWizard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketWizard.Infrastructure.UnitTests;

public class SqlMiscRepositoryUnitTest
{
    private static MarketWizardContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MarketWizardContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new MarketWizardContext(options);
    }

    [Fact]
    public async Task GetIndices_WhenCronJobExists_ReturnsDeserializedData()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var date = new DateTime(2026, 10, 7, 10, 0, 0, DateTimeKind.Utc);
        var indices = new List<IndicePerformance>
        {
            new() { Id = 1, Name = "USA", CountryCode = "USA", RegularMarketChangePercent = 1.25, RegularMarketPrice = 50000 },
            new() { Id = 2, Name = "GBR", CountryCode = "GBR", RegularMarketChangePercent = -0.5, RegularMarketPrice = 8000 }
        };

        var cronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = date,
            IndicePerfomance = JsonSerializer.Serialize(indices)
        };

        context.CronJobs.Add(cronJob);
        await context.SaveChangesAsync();

        var repo = new SqlMiscRepository(context);
        var result = await repo.GetIndices();

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("USA", result.Items[0].CountryCode);
        Assert.Equal(50000, result.Items[0].RegularMarketPrice);
        Assert.Equal(date, result.Date?.UtcDateTime);
    }

    [Fact]
    public async Task GetAllMethods_WhenMultipleCronJobsExist_ReturnsLatestCronJobData()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var olderDate = new DateTime(2026, 10, 6, 10, 0, 0, DateTimeKind.Utc);
        var newerDate = new DateTime(2026, 10, 7, 10, 0, 0, DateTimeKind.Utc);

        var olderCronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = olderDate,
            TopNews = JsonSerializer.Serialize(new List<TopNews> { new() { Id = 1, Text = "Old News" } }),
            SectorPerformance = JsonSerializer.Serialize(new List<SectorPerformance> { new() { Type = "Technology", Change = 0.1, Country = "USA" } }),
            Gainers = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Technology", Change = 0.5, Country = "USA" } }),
            Losers = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Energy", Change = -0.4, Country = "USA" } }),
            TopIndustries = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Semiconductors", Change = 0.8, Country = "USA" } }),
            WorstIndustries = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Oil & Gas", Change = -0.6, Country = "USA" } })
        };

        var newerCronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = newerDate,
            TopNews = JsonSerializer.Serialize(new List<TopNews> { new() { Id = 2, Text = "New News", Source = "Bloomberg" } }),
            SectorPerformance = JsonSerializer.Serialize(new List<SectorPerformance> { new() { Type = "Healthcare", Change = 0.3, Country = "USA" } }),
            Gainers = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Healthcare", Change = 0.7, Country = "USA" } }),
            Losers = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Utilities", Change = -0.2, Country = "USA" } }),
            TopIndustries = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Biotech", Change = 0.9, Country = "USA" } }),
            WorstIndustries = JsonSerializer.Serialize(new List<Gainers> { new() { Type = "Coal", Change = -0.8, Country = "USA" } })
        };

        context.CronJobs.AddRange(olderCronJob, newerCronJob);
        await context.SaveChangesAsync();

        var repo = new SqlMiscRepository(context);

        var topNews = await repo.GetTopNews();
        Assert.Single(topNews.Items);
        Assert.Equal("New News", topNews.Items[0].Text);
        Assert.Equal("Bloomberg", topNews.Items[0].Source);

        var sectorPerf = await repo.GetSectorPerformance();
        Assert.Single(sectorPerf.Items);
        Assert.Equal("Healthcare", sectorPerf.Items[0].Type);

        var gainers = await repo.GetTopGainers();
        Assert.Single(gainers.Items);
        Assert.Equal("Healthcare", gainers.Items[0].Type);

        var losers = await repo.GetTopLosers();
        Assert.Single(losers.Items);
        Assert.Equal("Utilities", losers.Items[0].Type);

        var topIndustries = await repo.GetTopIndustries();
        Assert.Single(topIndustries.Items);
        Assert.Equal("Biotech", topIndustries.Items[0].Type);

        var worstIndustries = await repo.GetWorstIndustries();
        Assert.Single(worstIndustries.Items);
        Assert.Equal("Coal", worstIndustries.Items[0].Type);
    }

    [Fact]
    public async Task GetAllMethods_WhenTableIsEmpty_ReturnsEmptyItemsWithoutThrowing()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var repo = new SqlMiscRepository(context);

        var indices = await repo.GetIndices();
        Assert.NotNull(indices);
        Assert.Empty(indices.Items);

        var news = await repo.GetTopNews();
        Assert.NotNull(news);
        Assert.Empty(news.Items);

        var sector = await repo.GetSectorPerformance();
        Assert.NotNull(sector);
        Assert.Empty(sector.Items);

        var gainers = await repo.GetTopGainers();
        Assert.NotNull(gainers);
        Assert.Empty(gainers.Items);

        var losers = await repo.GetTopLosers();
        Assert.NotNull(losers);
        Assert.Empty(losers.Items);

        var topIndustries = await repo.GetTopIndustries();
        Assert.NotNull(topIndustries);
        Assert.Empty(topIndustries.Items);

        var worstIndustries = await repo.GetWorstIndustries();
        Assert.NotNull(worstIndustries);
        Assert.Empty(worstIndustries.Items);
    }

    [Fact]
    public async Task GetAllMethods_WhenJsonIsWrappedInObject_ReturnsDeserializedData()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var date = new DateTime(2026, 10, 7, 12, 0, 0, DateTimeKind.Utc);
        var cronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = date,
            Gainers = "{\"Items\": [{\"Type\": \"Technology\", \"Change\": 0.42, \"Country\": \"USA\"}]}"
        };

        context.CronJobs.Add(cronJob);
        await context.SaveChangesAsync();

        var repo = new SqlMiscRepository(context);
        var gainers = await repo.GetTopGainers();

        Assert.NotNull(gainers);
        Assert.Single(gainers.Items);
        Assert.Equal("Technology", gainers.Items[0].Type);
        Assert.Equal(0.42, gainers.Items[0].Change);
    }

    [Fact]
    public async Task GetIndividualMethods_WhenOnlySpecificColumnIsPopulated_ReturnsOnlyPopulatedColumnData()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var date = new DateTime(2026, 10, 7, 14, 0, 0, DateTimeKind.Utc);
        var cronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = date,
            TopNews = JsonSerializer.Serialize(new List<TopNews> { new() { Id = 42, Text = "Targeted News" } }),
            SectorPerformance = null,
            Gainers = null
        };

        context.CronJobs.Add(cronJob);
        await context.SaveChangesAsync();

        var repo = new SqlMiscRepository(context);

        var news = await repo.GetTopNews();
        Assert.NotNull(news);
        Assert.Single(news.Items);
        Assert.Equal("Targeted News", news.Items[0].Text);

        var sector = await repo.GetSectorPerformance();
        Assert.NotNull(sector);
        Assert.Empty(sector.Items);
    }
}
