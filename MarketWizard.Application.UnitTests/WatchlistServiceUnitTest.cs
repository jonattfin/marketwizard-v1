using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using MarketWizard.Domain.Entities;
using NSubstitute;

namespace MarketWizard.Application.UnitTests;

public class WatchlistServiceUnitTest
{
    private readonly IWatchlistService _service;

    public WatchlistServiceUnitTest()
    {
        var repo = Substitute.For<IWatchlistRepository>();
        repo.GetAll().Returns([]);
        repo.DeleteWatchlist("1").Returns(true);
        repo.DeleteWatchlistItem(new WatchlistItemDto("1", "2")).Returns(true);

        _service = new WatchlistService(repo);
    }

    [Fact]
    public async Task GetAll()
    {
        // Assert
        Assert.Empty(await _service.GetAll());
    }

    [Fact]
    public async Task DeleteWatchlist()
    {
        // Assert
        Assert.True(await _service.DeleteWatchlist("1"));
    }

    [Fact]
    public async Task DeleteWatchlistItem()
    {
        // Assert
        Assert.True(await _service.DeleteWatchlistItem(new WatchlistItemDto("1", "2")));
    }
    
    [Fact]
    public async Task CreateWatchlist_ForwardsParameters()
    {
        var repo = Substitute.For<IWatchlistRepository>();
        var dto = new CreateWatchlistDto("My Tech Watchlist");
        var expected = new WatchlistType { Id = "123", Name = "My Tech Watchlist", Items = [] };
        repo.CreateWatchlist(dto, Arg.Any<CancellationToken>()).Returns(expected);

        var service = new WatchlistService(repo);
        var result = await service.CreateWatchlist(dto);

        Assert.NotNull(result);
        Assert.Equal("My Tech Watchlist", result.Name);
        await repo.Received(1).CreateWatchlist(dto, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAll_ForwardsCancellationToken()
    {
        var repo = Substitute.For<IWatchlistRepository>();
        var cancellationToken = new CancellationToken(true);
        repo.GetAll(cancellationToken).Returns([]);

        var service = new WatchlistService(repo);
        var result = await service.GetAll(cancellationToken);

        Assert.Empty(result);
        await repo.Received(1).GetAll(cancellationToken);
    }
}