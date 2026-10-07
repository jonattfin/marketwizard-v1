using System.Text.Encodings.Web;
using System.Text.Json;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarketWizard.Application.Services;

public class MarketDataSyncService(
    IMarketDataProvider marketDataProvider,
    ICronJobRepository cronJobRepository,
    ILogger<MarketDataSyncService> logger) : IMarketDataSyncService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public async Task SyncDailyMarketDataAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting daily market data synchronization.");

        try
        {
            var indicesTask = marketDataProvider.GetIndicesAsync(cancellationToken);
            var topNewsTask = marketDataProvider.GetTopNewsAsync(cancellationToken);
            var sectorPerfTask = marketDataProvider.GetSectorPerformanceAsync(cancellationToken);
            var topGainersTask = marketDataProvider.GetTopGainersAsync(cancellationToken);
            var topLosersTask = marketDataProvider.GetTopLosersAsync(cancellationToken);
            var topIndustriesTask = marketDataProvider.GetTopIndustriesAsync(cancellationToken);
            var worstIndustriesTask = marketDataProvider.GetWorstIndustriesAsync(cancellationToken);

            await Task.WhenAll(
                indicesTask,
                topNewsTask,
                sectorPerfTask,
                topGainersTask,
                topLosersTask,
                topIndustriesTask,
                worstIndustriesTask);

            var indices = await indicesTask;
            var topNews = await topNewsTask;
            var sectorPerf = await sectorPerfTask;
            var topGainers = await topGainersTask;
            var topLosers = await topLosersTask;
            var topIndustries = await topIndustriesTask;
            var worstIndustries = await worstIndustriesTask;

            var indicesJson = JsonSerializer.Serialize(indices?.Items ?? [], JsonOptions);
            var topNewsJson = JsonSerializer.Serialize(topNews?.Items ?? [], JsonOptions);
            var sectorPerfJson = JsonSerializer.Serialize(sectorPerf?.Items ?? [], JsonOptions);
            var topGainersJson = JsonSerializer.Serialize(topGainers?.Items ?? [], JsonOptions);
            var topLosersJson = JsonSerializer.Serialize(topLosers?.Items ?? [], JsonOptions);
            var topIndustriesJson = JsonSerializer.Serialize(topIndustries?.Items ?? [], JsonOptions);
            var worstIndustriesJson = JsonSerializer.Serialize(worstIndustries?.Items ?? [], JsonOptions);

            var snapshotDate = DateTime.UtcNow;

            await cronJobRepository.SaveDailySnapshotAsync(
                indicesJson,
                topNewsJson,
                sectorPerfJson,
                topGainersJson,
                topLosersJson,
                topIndustriesJson,
                worstIndustriesJson,
                snapshotDate,
                cancellationToken);

            logger.LogInformation("Daily market data synchronization completed successfully at {Date}.", snapshotDate);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "An error occurred while synchronizing daily market data.");
            throw;
        }
    }
}
