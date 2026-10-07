using Infrastructure.BackgroundServices;
using Infrastructure.Configuration;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MarketWizard.Infrastructure.UnitTests;

public class MarketDataSyncBackgroundServiceUnitTest
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMarketDataSyncService _syncService;
    private readonly ILogger<MarketDataSyncBackgroundService> _logger;

    public MarketDataSyncBackgroundServiceUnitTest()
    {
        _scopeFactory = Substitute.For<IServiceScopeFactory>();
        _scope = Substitute.For<IServiceScope>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        _syncService = Substitute.For<IMarketDataSyncService>();
        _logger = Substitute.For<ILogger<MarketDataSyncBackgroundService>>();

        _scopeFactory.CreateScope().Returns(_scope);
        _scope.ServiceProvider.Returns(_serviceProvider);
        _serviceProvider.GetService(typeof(IMarketDataSyncService)).Returns(_syncService);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDisabled_DoesNotExecuteSync()
    {
        // Arrange
        var options = Options.Create(new MarketDataSyncOptions
        {
            Enabled = false,
            RunOnStartup = true,
            InitialDelaySeconds = 0
        });

        var service = new MarketDataSyncBackgroundService(_scopeFactory, options, _logger);
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        // Act
        await service.StartAsync(cts.Token);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _scopeFactory.DidNotReceive().CreateScope();
        await _syncService.DidNotReceiveWithAnyArgs().SyncDailyMarketDataAsync(default);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRunOnStartupTrue_ExecutesSyncOnceOnStartup()
    {
        // Arrange
        var options = Options.Create(new MarketDataSyncOptions
        {
            Enabled = true,
            RunOnStartup = true,
            InitialDelaySeconds = 0,
            IntervalHours = 24.0
        });

        var service = new MarketDataSyncBackgroundService(_scopeFactory, options, _logger);
        using var cts = new CancellationTokenSource();

        // Act
        var startTask = service.StartAsync(cts.Token);
        await Task.Delay(50);
        cts.Cancel();
        await service.StopAsync(CancellationToken.None);
        await startTask;

        // Assert
        _scopeFactory.Received(1).CreateScope();
        await _syncService.Received(1).SyncDailyMarketDataAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenSyncThrowsException_CatchesAndDoesNotTerminateHost()
    {
        // Arrange
        _syncService.SyncDailyMarketDataAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Transient database network error"));

        var options = Options.Create(new MarketDataSyncOptions
        {
            Enabled = true,
            RunOnStartup = true,
            InitialDelaySeconds = 0,
            IntervalHours = 24.0
        });

        var service = new MarketDataSyncBackgroundService(_scopeFactory, options, _logger);
        using var cts = new CancellationTokenSource();

        // Act & Assert (Should not throw)
        var startTask = service.StartAsync(cts.Token);
        await Task.Delay(50);
        cts.Cancel();
        await service.StopAsync(CancellationToken.None);
        await startTask;

        _scopeFactory.Received(1).CreateScope();
        await _syncService.Received(1).SyncDailyMarketDataAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenCancelledDuringInitialDelay_ExitsGracefully()
    {
        // Arrange
        var options = Options.Create(new MarketDataSyncOptions
        {
            Enabled = true,
            RunOnStartup = true,
            InitialDelaySeconds = 60,
            IntervalHours = 24.0
        });

        var service = new MarketDataSyncBackgroundService(_scopeFactory, options, _logger);
        using var cts = new CancellationTokenSource();

        // Act
        var startTask = service.StartAsync(cts.Token);
        await Task.Delay(20);
        cts.Cancel();
        await service.StopAsync(CancellationToken.None);
        await startTask;

        // Assert
        _scopeFactory.DidNotReceive().CreateScope();
        await _syncService.DidNotReceiveWithAnyArgs().SyncDailyMarketDataAsync(default);
    }
}
