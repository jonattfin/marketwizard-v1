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
    
}