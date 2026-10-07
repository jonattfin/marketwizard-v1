namespace MarketWizard.Application.Interfaces;

public interface IMarketDataSyncService
{
    Task SyncDailyMarketDataAsync(CancellationToken cancellationToken = default);
}
