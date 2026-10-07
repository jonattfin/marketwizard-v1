using System.Text.Json;
using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using MarketWizard.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MarketWizard.Application.UnitTests;

public class MarketDataSyncServiceUnitTest
{
    private readonly IMarketDataProvider _provider;
    private readonly ICronJobRepository _cronJobRepository;
    private readonly ILogger<MarketDataSyncService> _logger;
    private readonly MarketDataSyncService _service;

    public MarketDataSyncServiceUnitTest()
    {
        _provider = Substitute.For<IMarketDataProvider>();
        _cronJobRepository = Substitute.For<ICronJobRepository>();
        _logger = Substitute.For<ILogger<MarketDataSyncService>>();

        _service = new MarketDataSyncService(_provider, _cronJobRepository, _logger);
    }

    [Fact]
    public async Task SyncDailyMarketDataAsync_WhenProviderReturnsData_SerializesAndPersistsAllDatasets()
    {
        // Arrange
        var indices = new IndicePerformanceData
        {
            Items = [new IndicePerformance { Id = 1, Name = "S&P 500", CountryCode = "USA", RegularMarketPrice = 5000, RegularMarketChangePercent = 0.5 }]
        };
        var news = new TopNewsData
        {
            Items = [new TopNews { Id = 1, Text = "Markets rally", Source = "Bloomberg" }]
        };
        var sectors = new SectorPerformanceData
        {
            Items = [new SectorPerformance { Type = "Technology", Change = 1.2, Country = "USA" }]
        };
        var gainers = new GainersData
        {
            Items = [new Gainers { Type = "NVDA", Change = 4.5, Country = "USA" }]
        };
        var losers = new GainersData
        {
            Items = [new Gainers { Type = "TSLA", Change = -2.1, Country = "USA" }]
        };
        var topIndustries = new GainersData
        {
            Items = [new Gainers { Type = "Semiconductors", Change = 3.0, Country = "USA" }]
        };
        var worstIndustries = new GainersData
        {
            Items = [new Gainers { Type = "Coal", Change = -1.5, Country = "USA" }]
        };

        _provider.GetIndicesAsync(Arg.Any<CancellationToken>()).Returns(indices);
        _provider.GetTopNewsAsync(Arg.Any<CancellationToken>()).Returns(news);
        _provider.GetSectorPerformanceAsync(Arg.Any<CancellationToken>()).Returns(sectors);
        _provider.GetTopGainersAsync(Arg.Any<CancellationToken>()).Returns(gainers);
        _provider.GetTopLosersAsync(Arg.Any<CancellationToken>()).Returns(losers);
        _provider.GetTopIndustriesAsync(Arg.Any<CancellationToken>()).Returns(topIndustries);
        _provider.GetWorstIndustriesAsync(Arg.Any<CancellationToken>()).Returns(worstIndustries);

        var beforeCall = DateTime.UtcNow;

        // Act
        await _service.SyncDailyMarketDataAsync(CancellationToken.None);

        var afterCall = DateTime.UtcNow;

        // Assert
        await _cronJobRepository.Received(1).SaveDailySnapshotAsync(
            Arg.Is<string>(s => s.Contains("S&P 500")),
            Arg.Is<string>(s => s.Contains("Markets rally")),
            Arg.Is<string>(s => s.Contains("Technology")),
            Arg.Is<string>(s => s.Contains("NVDA")),
            Arg.Is<string>(s => s.Contains("TSLA")),
            Arg.Is<string>(s => s.Contains("Semiconductors")),
            Arg.Is<string>(s => s.Contains("Coal")),
            Arg.Is<DateTime>(d => d >= beforeCall && d <= afterCall),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncDailyMarketDataAsync_WhenProviderReturnsNullOrEmptyItems_SerializesEmptyArrays()
    {
        // Arrange
        _provider.GetIndicesAsync(Arg.Any<CancellationToken>()).Returns(new IndicePerformanceData { Items = [] });
        _provider.GetTopNewsAsync(Arg.Any<CancellationToken>()).Returns(new TopNewsData { Items = [] });
        _provider.GetSectorPerformanceAsync(Arg.Any<CancellationToken>()).Returns(new SectorPerformanceData { Items = [] });
        _provider.GetTopGainersAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetTopLosersAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetTopIndustriesAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetWorstIndustriesAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });

        // Act
        await _service.SyncDailyMarketDataAsync(CancellationToken.None);

        // Assert
        await _cronJobRepository.Received(1).SaveDailySnapshotAsync(
            "[]",
            "[]",
            "[]",
            "[]",
            "[]",
            "[]",
            "[]",
            Arg.Any<DateTime>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncDailyMarketDataAsync_WhenProviderThrows_PropagatesExceptionAndDoesNotPersist()
    {
        // Arrange
        _provider.GetIndicesAsync(Arg.Any<CancellationToken>()).ThrowsAsync(new InvalidOperationException("Provider offline"));
        _provider.GetTopNewsAsync(Arg.Any<CancellationToken>()).Returns(new TopNewsData { Items = [] });
        _provider.GetSectorPerformanceAsync(Arg.Any<CancellationToken>()).Returns(new SectorPerformanceData { Items = [] });
        _provider.GetTopGainersAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetTopLosersAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetTopIndustriesAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });
        _provider.GetWorstIndustriesAsync(Arg.Any<CancellationToken>()).Returns(new GainersData { Items = [] });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.SyncDailyMarketDataAsync(CancellationToken.None));
        Assert.Equal("Provider offline", ex.Message);

        await _cronJobRepository.DidNotReceiveWithAnyArgs().SaveDailySnapshotAsync(
            default!, default!, default!, default!, default!, default!, default!, default, default);
    }
}
