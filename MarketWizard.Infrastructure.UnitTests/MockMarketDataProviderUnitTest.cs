using Infrastructure.Services;

namespace MarketWizard.Infrastructure.UnitTests;

public class MockMarketDataProviderUnitTest
{
    private readonly MockMarketDataProvider _provider = new();

    [Fact]
    public async Task GetIndicesAsync_ReturnsPopulatedIndices()
    {
        var result = await _provider.GetIndicesAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Name));
            Assert.False(string.IsNullOrWhiteSpace(item.CountryCode));
            Assert.True(item.RegularMarketPrice > 0);
        });
    }

    [Fact]
    public async Task GetTopNewsAsync_ReturnsPopulatedNewsItems()
    {
        var result = await _provider.GetTopNewsAsync();

        Assert.NotNull(result);
        Assert.Equal(10, result.Items.Count);
        Assert.All(result.Items, item =>
        {
            Assert.True(item.Id > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.Text));
            Assert.False(string.IsNullOrWhiteSpace(item.Source));
            Assert.NotNull(item.Sentiment);
        });
    }

    [Fact]
    public async Task GetSectorPerformanceAsync_ReturnsAllSectors()
    {
        var result = await _provider.GetSectorPerformanceAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Type));
            Assert.Equal("USA", item.Country);
        });
    }

    [Fact]
    public async Task GetTopGainersAsync_ReturnsPositiveGainers()
    {
        var result = await _provider.GetTopGainersAsync();

        Assert.NotNull(result);
        Assert.Equal(6, result.Items.Count);
        Assert.All(result.Items, item =>
        {
            Assert.True(item.Change > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.Type));
        });
    }

    [Fact]
    public async Task GetTopLosersAsync_ReturnsNegativeGainers()
    {
        var result = await _provider.GetTopLosersAsync();

        Assert.NotNull(result);
        Assert.Equal(6, result.Items.Count);
        Assert.All(result.Items, item =>
        {
            Assert.True(item.Change < 0);
            Assert.False(string.IsNullOrWhiteSpace(item.Type));
        });
    }

    [Fact]
    public async Task GetTopIndustriesAsync_ReturnsPositiveGainers()
    {
        var result = await _provider.GetTopIndustriesAsync();

        Assert.NotNull(result);
        Assert.Equal(6, result.Items.Count);
        Assert.All(result.Items, item =>
        {
            Assert.True(item.Change > 0);
        });
    }

    [Fact]
    public async Task GetWorstIndustriesAsync_ReturnsNegativeGainers()
    {
        var result = await _provider.GetWorstIndustriesAsync();

        Assert.NotNull(result);
        Assert.Equal(6, result.Items.Count);
        Assert.All(result.Items, item =>
        {
            Assert.True(item.Change < 0);
        });
    }
}
