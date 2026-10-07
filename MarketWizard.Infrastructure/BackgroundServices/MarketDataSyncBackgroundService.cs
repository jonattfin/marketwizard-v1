using Infrastructure.Configuration;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices;

public class MarketDataSyncBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<MarketDataSyncOptions> options,
    ILogger<MarketDataSyncBackgroundService> logger) : BackgroundService
{
    private readonly MarketDataSyncOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("MarketDataSyncBackgroundService is disabled by configuration.");
            return;
        }

        logger.LogInformation(
            "MarketDataSyncBackgroundService is starting with interval {IntervalHours}h, RunOnStartup: {RunOnStartup}, InitialDelay: {InitialDelaySeconds}s.",
            _options.IntervalHours,
            _options.RunOnStartup,
            _options.InitialDelaySeconds);

        if (_options.InitialDelaySeconds > 0)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_options.InitialDelaySeconds), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("MarketDataSyncBackgroundService stopped during initial delay.");
                return;
            }
        }

        if (_options.RunOnStartup)
        {
            await ExecuteSyncCycleAsync(stoppingToken);
        }

        var interval = TimeSpan.FromHours(Math.Max(_options.IntervalHours, 0.0001));
        using var timer = new PeriodicTimer(interval);

        try
        {
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ExecuteSyncCycleAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("MarketDataSyncBackgroundService has received cancellation request.");
        }
    }

    private async Task ExecuteSyncCycleAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            logger.LogInformation("Executing market data synchronization cycle.");
            using var scope = scopeFactory.CreateScope();
            var syncService = scope.ServiceProvider.GetRequiredService<IMarketDataSyncService>();
            await syncService.SyncDailyMarketDataAsync(cancellationToken);
            logger.LogInformation("Market data synchronization cycle executed successfully.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Market data synchronization cycle was cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during market data synchronization cycle. Service will retry on next interval.");
        }
    }
}
