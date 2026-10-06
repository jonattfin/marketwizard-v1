using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Interfaces;

public interface IWatchlistRepository
{
    Task<List<WatchlistType>> GetAll();
    Task CreateWatchlist();
    Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto);
    Task DeleteWatchlist(string id);
    Task CreateWatchlistItem(WatchlistItemDto dto);
    Task DeleteWatchlistItem(WatchlistItemDto dto);
}