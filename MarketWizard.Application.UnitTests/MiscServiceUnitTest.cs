using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using MarketWizard.Domain.Entities;
using NSubstitute;

namespace MarketWizard.Application.UnitTests;

public class MiscServiceUnitTest
{
    private readonly IMiscService _service;

    public MiscServiceUnitTest()
    {
        var repo = Substitute.For<IMiscRepository>();
        repo.GetIndices().Returns(new IndicePerformanceData() { Items = [] });
        repo.GetTopNews().Returns(new TopNewsData() { Items = [] });
        repo.GetSectorPerformance().Returns(new SectorPerformanceData() { Items = [] });
        repo.GetTopGainers().Returns(new GainersData() { Items = [] });
        repo.GetTopLosers().Returns(new GainersData() { Items = [] });
        repo.GetTopIndustries().Returns(new GainersData() { Items = [] });
        repo.GetWorstIndustries().Returns(new GainersData() { Items = [] });

        _service = new MiscService(repo);
    }

    [Fact]
    public async Task GetIndices()
    {
        // Assert
        var data = await _service.GetIndices();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetTopNews()
    {
        // Assert
        var data = await _service.GetTopNews();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetSectorPerformance()
    {
        // Assert
        var data = await _service.GetSectorPerformance();

        Assert.Empty(data.Items);
    }
    
    [Fact]
    public async Task GetTopGainers()
    {
        // Assert
        var data = await _service.GetTopGainers();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetTopLosers()
    {
        // Assert
        var data = await _service.GetTopLosers();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetTopIndustries()
    {
        // Assert
        var data = await _service.GetTopIndustries();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetWorstIndustries()
    {
        // Assert
        var data = await _service.GetWorstIndustries();

        Assert.Empty(data.Items);
    }

    [Fact]
    public async Task GetIndices_ForwardsCancellationToken()
    {
        var repo = Substitute.For<IMiscRepository>();
        var cancellationToken = new CancellationToken(true);
        repo.GetIndices(cancellationToken).Returns(new IndicePerformanceData { Items = [] });

        var service = new MiscService(repo);
        var data = await service.GetIndices(cancellationToken);

        Assert.Empty(data.Items);
        await repo.Received(1).GetIndices(cancellationToken);
    }
}