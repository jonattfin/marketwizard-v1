namespace MarketWizard.Application.Interfaces;

public interface ICronJobRepository
{
    Task SaveDailySnapshotAsync(
        string indicesJson,
        string topNewsJson,
        string sectorPerformanceJson,
        string gainersJson,
        string losersJson,
        string topIndustriesJson,
        string worstIndustriesJson,
        DateTime date,
        CancellationToken cancellationToken = default);
}
