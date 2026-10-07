using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using MarketWizard.Application.Interfaces;

namespace Infrastructure.Repositories;

public class CronJobRepository(MarketWizardContext context) : ICronJobRepository
{
    public async Task SaveDailySnapshotAsync(
        string indicesJson,
        string topNewsJson,
        string sectorPerformanceJson,
        string gainersJson,
        string losersJson,
        string topIndustriesJson,
        string worstIndustriesJson,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var cronJob = new CronJob
        {
            Id = Guid.NewGuid(),
            Date = date,
            IndicePerfomance = indicesJson,
            TopNews = topNewsJson,
            SectorPerformance = sectorPerformanceJson,
            Gainers = gainersJson,
            Losers = losersJson,
            TopIndustries = topIndustriesJson,
            WorstIndustries = worstIndustriesJson
        };

        context.CronJobs.Add(cronJob);
        await context.SaveChangesAsync(cancellationToken);
    }
}
