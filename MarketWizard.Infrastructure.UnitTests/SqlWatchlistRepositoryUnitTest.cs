using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using Infrastructure.Repositories;
using MarketWizard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketWizard.Infrastructure.UnitTests;

public class SqlWatchlistRepositoryUnitTest
{
    private static MarketWizardContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MarketWizardContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new MarketWizardContext(options);
    }

    [Fact]
    public async Task GetAll_WhenEmpty_ReturnsEmptyList()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var result = await repo.GetAll();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAll_WhenWatchlistsExist_ReturnsMappedWatchlistsWithItems()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var watchlistId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var watchlist = new Watchlist
        {
            Id = watchlistId,
            Name = "Tech Watchlist",
            Items =
            [
                new WatchlistItem
                {
                    Id = itemId,
                    WatchlistId = watchlistId,
                    Name = "AAPL",
                    Description = "Apple Inc.",
                    Ticker = "AAPL"
                }
            ]
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);
        var result = await repo.GetAll();

        Assert.Single(result);
        Assert.Equal(watchlistId.ToString(), result[0].Id);
        Assert.Equal("Tech Watchlist", result[0].Name);
        Assert.Single(result[0].Items);
        Assert.Equal(itemId.ToString(), result[0].Items[0].Id);
        Assert.Equal("AAPL", result[0].Items[0].Name);
        Assert.Equal("Apple Inc.", result[0].Items[0].Description);
        Assert.Equal("AAPL", result[0].Items[0].Ticker);
    }

    [Fact]
    public async Task GetPaged_ReturnsCorrectPageAndMetadata()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        for (var i = 1; i <= 7; i++)
        {
            context.Watchlists.Add(new Watchlist
            {
                Id = Guid.NewGuid(),
                Name = $"Watchlist {i:D2}",
                Items = []
            });
        }

        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);

        // Page 1 with pageSize 3
        var page1 = await repo.GetPaged(1, 3);
        Assert.Equal(3, page1.Items.Count);
        Assert.Equal(1, page1.PageNumber);
        Assert.Equal(3, page1.PageSize);
        Assert.Equal(7, page1.TotalCount);
        Assert.Equal(3, page1.TotalPages);
        Assert.False(page1.HasPreviousPage);
        Assert.True(page1.HasNextPage);
        Assert.Equal("Watchlist 01", page1.Items[0].Name);

        // Page 3 with pageSize 3
        var page3 = await repo.GetPaged(3, 3);
        Assert.Single(page3.Items);
        Assert.Equal(3, page3.PageNumber);
        Assert.Equal(3, page3.PageSize);
        Assert.Equal(7, page3.TotalCount);
        Assert.Equal(3, page3.TotalPages);
        Assert.True(page3.HasPreviousPage);
        Assert.False(page3.HasNextPage);
        Assert.Equal("Watchlist 07", page3.Items[0].Name);
    }

    [Fact]
    public async Task GetPaged_WhenEmpty_ReturnsEmptyPagedResult()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var result = await repo.GetPaged(1, 10);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public async Task CreateWatchlist_AddsNewWatchlistToDatabase()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var result = await repo.CreateWatchlist();

        var watchlists = await context.Watchlists.ToListAsync();
        Assert.Single(watchlists);
        Assert.StartsWith("Watchlist ", watchlists[0].Name);
        Assert.NotEqual(Guid.Empty, watchlists[0].Id);
        Assert.Equal(watchlists[0].Id.ToString(), result.Id);
        Assert.Equal(watchlists[0].Name, result.Name);
    }

    [Fact]
    public async Task CreateWatchlist_WithCustomName_SetsCustomName()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var result = await repo.CreateWatchlist(new CreateWatchlistDto("My Tech Watchlist"));

        var watchlists = await context.Watchlists.ToListAsync();
        Assert.Single(watchlists);
        Assert.Equal("My Tech Watchlist", watchlists[0].Name);
        Assert.Equal("My Tech Watchlist", result.Name);
    }

    [Fact]
    public async Task UpdateWatchlist_WhenWatchlistExists_UpdatesNameAndReturnsUpdatedDto()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var watchlist = new Watchlist
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Items = []
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);
        var result = await repo.UpdateWatchlist(watchlist.Id.ToString(), new UpdateWatchlistDto("Updated Name"));

        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);

        var updatedEntity = await context.Watchlists.FindAsync(watchlist.Id);
        Assert.Equal("Updated Name", updatedEntity?.Name);
    }

    [Fact]
    public async Task UpdateWatchlist_WhenInvalidGuidOrNotFound_ReturnsNull()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var invalidGuidResult = await repo.UpdateWatchlist("not-a-guid", new UpdateWatchlistDto("Updated Name"));
        Assert.Null(invalidGuidResult);

        var notFoundResult =
            await repo.UpdateWatchlist(Guid.NewGuid().ToString(), new UpdateWatchlistDto("Updated Name"));
        Assert.Null(notFoundResult);
    }

    [Fact]
    public async Task DeleteWatchlist_WhenWatchlistExists_DeletesWatchlistAndReturnsTrue()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var watchlist = new Watchlist
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            Items = []
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);
        var result = await repo.DeleteWatchlist(watchlist.Id.ToString());

        Assert.True(result);
        var remaining = await context.Watchlists.ToListAsync();
        Assert.Empty(remaining);
    }

    [Fact]
    public async Task DeleteWatchlist_WhenInvalidGuidOrNotFound_ReturnsFalse()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        var invalidGuidResult = await repo.DeleteWatchlist("invalid-guid");
        Assert.False(invalidGuidResult);

        var notFoundResult = await repo.DeleteWatchlist(Guid.NewGuid().ToString());
        Assert.False(notFoundResult);
    }

    [Fact]
    public async Task CreateWatchlistItem_WhenWatchlistExists_AddsItemToWatchlist()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var watchlist = new Watchlist
        {
            Id = Guid.NewGuid(),
            Name = "My Portfolio",
            Items = []
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);
        await repo.CreateWatchlistItem(new WatchlistItemDto(watchlist.Id.ToString(), "MSFT"));

        var items = await context.WatchlistItems.ToListAsync();
        Assert.Single(items);
        Assert.Equal("MSFT", items[0].Ticker);
        Assert.Equal("MSFT", items[0].Name);
        Assert.Equal(watchlist.Id, items[0].WatchlistId);
    }

    [Fact]
    public async Task CreateWatchlistItem_WhenWatchlistDoesNotExist_DoesNotAddItem()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        await repo.CreateWatchlistItem(new WatchlistItemDto(Guid.NewGuid().ToString(), "NVDA"));

        var items = await context.WatchlistItems.ToListAsync();
        Assert.Empty(items);
    }

    [Fact]
    public async Task DeleteWatchlistItem_WhenItemMatchesTicker_RemovesItemAndReturnsTrue()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var watchlistId = Guid.NewGuid();
        var watchlist = new Watchlist
        {
            Id = watchlistId,
            Name = "Crypto",
            Items =
            [
                new WatchlistItem
                {
                    Id = Guid.NewGuid(),
                    WatchlistId = watchlistId,
                    Ticker = "BTC",
                    Name = "BTC",
                    Description = "Description"
                }
            ]
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync();

        var repo = new SqlWatchlistRepository(context);
        var result = await repo.DeleteWatchlistItem(new WatchlistItemDto(watchlistId.ToString(), "BTC"));

        Assert.True(result);
        var items = await context.WatchlistItems.ToListAsync();
        Assert.Empty(items);
    }

    [Fact]
    public async Task DeleteWatchlistItem_WhenItemDoesNotExist_ReturnsFalse()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);

        var repo = new SqlWatchlistRepository(context);
        var result = await repo.DeleteWatchlistItem(new WatchlistItemDto(Guid.NewGuid().ToString(), "NONEXISTENT"));

        Assert.False(result);
    }

    [Fact]
    public async Task GetAll_WhenCancellationTokenIsCancelled_ThrowsOperationCanceledException()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = CreateContext(dbName);
        var repo = new SqlWatchlistRepository(context);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => repo.GetAll(cts.Token));
    }
}