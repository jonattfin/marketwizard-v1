using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Services;

public interface IWatchlistService
{
    Task<List<WatchlistType>> GetAll();
    Task CreateWatchlist();
    Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto);
    Task DeleteWatchlist(string id);
    Task CreateWatchlistItem(WatchlistItemDto dto);
    Task DeleteWatchlistItem(WatchlistItemDto dto);
}

public class WatchlistService(IWatchlistRepository watchlistRepository) : IWatchlistService
{
    public Task<List<WatchlistType>> GetAll() => watchlistRepository.GetAll();
    public Task CreateWatchlist() => watchlistRepository.CreateWatchlist();
    public Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto) => watchlistRepository.UpdateWatchlist(id, dto);
    public Task DeleteWatchlist(string id) => watchlistRepository.DeleteWatchlist(id);

    public Task CreateWatchlistItem(WatchlistItemDto dto) => watchlistRepository.CreateWatchlistItem(dto);
    public Task DeleteWatchlistItem(WatchlistItemDto dto) => watchlistRepository.DeleteWatchlistItem(dto);
}