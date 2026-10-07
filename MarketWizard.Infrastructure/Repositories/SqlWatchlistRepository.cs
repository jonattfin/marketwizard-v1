using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlWatchlistRepository(MarketWizardContext context) : IWatchlistRepository
{
    public async Task<List<WatchlistType>> GetAll(CancellationToken cancellationToken = default)
    {
        var watchlists = await context.Watchlists
            .AsNoTracking()
            .Include(w => w.Items)
            .OrderBy(w => w.Name)
            .ToListAsync(cancellationToken);

        return watchlists.Select(w => new WatchlistType
        {
            Id = w.Id.ToString(),
            Name = w.Name,
            Items = w.Items.Select(i => new WatchlistItemType
            {
                Id = i.Id.ToString(),
                Name = i.Name,
                Description = i.Description,
                Ticker = i.Ticker
            }).ToList()
        }).ToList();
    }

    public async Task<WatchlistType> CreateWatchlist(CreateWatchlistDto? dto = null, CancellationToken cancellationToken = default)
    {
        var name = string.IsNullOrWhiteSpace(dto?.Name) ? "Watchlist " + Guid.NewGuid() : dto.Name;
        var watchlist = new Watchlist
        {
            Id = Guid.NewGuid(),
            Name = name,
            Items = []
        };

        context.Watchlists.Add(watchlist);
        await context.SaveChangesAsync(cancellationToken);

        return new WatchlistType
        {
            Id = watchlist.Id.ToString(),
            Name = watchlist.Name,
            Items = []
        };
    }

    public async Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return null;
        }

        var watchlist = await context.Watchlists
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.Id == guid, cancellationToken);

        if (watchlist is null)
        {
            return null;
        }

        watchlist.Name = dto.Name;
        await context.SaveChangesAsync(cancellationToken);

        return new WatchlistType
        {
            Id = watchlist.Id.ToString(),
            Name = watchlist.Name,
            Items = watchlist.Items.Select(i => new WatchlistItemType
            {
                Id = i.Id.ToString(),
                Name = i.Name,
                Description = i.Description,
                Ticker = i.Ticker
            }).ToList()
        };
    }

    public async Task<bool> DeleteWatchlist(string id, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return false;
        }

        var watchlist = await context.Watchlists.FindAsync([guid], cancellationToken);
        if (watchlist is null)
        {
            return false;
        }

        context.Watchlists.Remove(watchlist);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task CreateWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default)
    {
        var rawWatchlistId = dto.WatchlistId ?? dto.Id;
        if (string.IsNullOrWhiteSpace(rawWatchlistId) || !Guid.TryParse(rawWatchlistId, out var watchlistId))
        {
            return;
        }

        var watchlistExists = await context.Watchlists.AnyAsync(w => w.Id == watchlistId, cancellationToken);
        if (!watchlistExists)
        {
            return;
        }

        var ticker = dto.Ticker ?? string.Empty;
        var item = new WatchlistItem
        {
            Id = Guid.NewGuid(),
            WatchlistId = watchlistId,
            Name = ticker,
            Description = "Description",
            Ticker = ticker
        };

        context.WatchlistItems.Add(item);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default)
    {
        var rawWatchlistId = dto.WatchlistId ?? dto.Id;
        if (string.IsNullOrWhiteSpace(rawWatchlistId) || !Guid.TryParse(rawWatchlistId, out var watchlistId))
        {
            return false;
        }

        var rawItemOrTicker = dto.ItemId ?? dto.Ticker;
        if (string.IsNullOrWhiteSpace(rawItemOrTicker))
        {
            return false;
        }

        var isItemId = Guid.TryParse(rawItemOrTicker, out var itemId);

        var item = await context.WatchlistItems
            .FirstOrDefaultAsync(i => i.WatchlistId == watchlistId && (i.Ticker == rawItemOrTicker || (isItemId && i.Id == itemId)), cancellationToken);

        if (item is null)
        {
            return false;
        }

        context.WatchlistItems.Remove(item);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
