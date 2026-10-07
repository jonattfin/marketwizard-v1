using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketWizard.Infrastructure.UnitTests;

public class CronJobRepositoryUnitTest
{
    private static MarketWizardContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MarketWizardContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new MarketWizardContext(options);
    }

    [Fact]
    public async Task SaveDailySnapshotAsync_PersistsNewCronJobRecord()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repository = new CronJobRepository(context);

        var date = new DateTime(2026, 10, 7, 12, 0, 0, DateTimeKind.Utc);
        var indicesJson = "[{\"name\":\"USA\"}]";
        var newsJson = "[{\"text\":\"News\"}]";
        var sectorJson = "[{\"type\":\"Tech\"}]";
        var gainersJson = "[{\"type\":\"Tech\"}]";
        var losersJson = "[{\"type\":\"Oil\"}]";
        var topIndJson = "[{\"type\":\"Semis\"}]";
        var worstIndJson = "[{\"type\":\"Coal\"}]";

        // Act
        await repository.SaveDailySnapshotAsync(
            indicesJson,
            newsJson,
            sectorJson,
            gainersJson,
            losersJson,
            topIndJson,
            worstIndJson,
            date,
            CancellationToken.None);

        // Assert
        var snapshot = await context.CronJobs.FirstOrDefaultAsync();
        Assert.NotNull(snapshot);
        Assert.NotEqual(Guid.Empty, snapshot.Id);
        Assert.Equal(date, snapshot.Date);
        Assert.Equal(indicesJson, snapshot.IndicePerfomance);
        Assert.Equal(newsJson, snapshot.TopNews);
        Assert.Equal(sectorJson, snapshot.SectorPerformance);
        Assert.Equal(gainersJson, snapshot.Gainers);
        Assert.Equal(losersJson, snapshot.Losers);
        Assert.Equal(topIndJson, snapshot.TopIndustries);
        Assert.Equal(worstIndJson, snapshot.WorstIndustries);
    }
}
