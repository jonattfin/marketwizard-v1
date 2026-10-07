using MarketWizard.Application.Features.MarketDataSync.Commands;
using MarketWizard.Application.Features.Misc.Queries;
using MarketWizard.Application.Features.Watchlists.Commands;
using MarketWizard.Application.Features.Watchlists.Queries;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using MediatR;
using NSubstitute;

namespace MarketWizard.Application.UnitTests;

public class MediatRHandlersUnitTest
{
    [Fact]
    public async Task MiscQueryHandlers_DelegateToRepository()
    {
        var repo = Substitute.For<IMiscRepository>();
        var indicesData = new IndicePerformanceData { Items = [] };
        repo.GetIndices(Arg.Any<CancellationToken>()).Returns(indicesData);

        var handlers = new MiscQueryHandlers(repo);
        var result = await handlers.Handle(new GetIndicesQuery(), CancellationToken.None);

        Assert.Same(indicesData, result);
        await repo.Received(1).GetIndices(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetWatchlistsQueryHandler_DelegatesToRepository()
    {
        var repo = Substitute.For<IWatchlistRepository>();
        var watchlists = new List<WatchlistType> { new() { Id = "1", Name = "W1", Items = [] } };
        repo.GetAll(Arg.Any<CancellationToken>()).Returns(watchlists);

        var handler = new GetWatchlistsQueryHandler(repo);
        var result = await handler.Handle(new GetWatchlistsQuery(), CancellationToken.None);

        Assert.Same(watchlists, result);
        await repo.Received(1).GetAll(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WatchlistCommandHandlers_DelegateToRepository()
    {
        var repo = Substitute.For<IWatchlistRepository>();
        var dto = new CreateWatchlistDto("My Tech");
        var created = new WatchlistType { Id = "123", Name = "My Tech", Items = [] };
        repo.CreateWatchlist(dto, Arg.Any<CancellationToken>()).Returns(created);

        var handlers = new WatchlistCommandHandlers(repo);
        var result = await handlers.Handle(new CreateWatchlistCommand(dto), CancellationToken.None);

        Assert.Same(created, result);
        await repo.Received(1).CreateWatchlist(dto, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncDailyMarketDataCommandHandler_DelegatesToSyncService()
    {
        var syncService = Substitute.For<IMarketDataSyncService>();
        var handler = new SyncDailyMarketDataCommandHandler(syncService);

        var result = await handler.Handle(new SyncDailyMarketDataCommand(), CancellationToken.None);

        Assert.Equal(Unit.Value, result);
        await syncService.Received(1).SyncDailyMarketDataAsync(Arg.Any<CancellationToken>());
    }
}
