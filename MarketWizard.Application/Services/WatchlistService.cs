using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Services;

public interface IWatchlistService
{
    Task<List<WatchlistType>> GetAll(CancellationToken cancellationToken = default);
    Task<WatchlistType> CreateWatchlist(CreateWatchlistDto? dto = null, CancellationToken cancellationToken = default);
    Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteWatchlist(string id, CancellationToken cancellationToken = default);
    Task CreateWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default);
}

public class WatchlistService(IWatchlistRepository watchlistRepository) : IWatchlistService
{
    public Task<List<WatchlistType>> GetAll(CancellationToken cancellationToken = default) => watchlistRepository.GetAll(cancellationToken);
    public Task<WatchlistType> CreateWatchlist(CreateWatchlistDto? dto = null, CancellationToken cancellationToken = default) => watchlistRepository.CreateWatchlist(dto, cancellationToken);
    public Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto, CancellationToken cancellationToken = default) => watchlistRepository.UpdateWatchlist(id, dto, cancellationToken);
    public Task<bool> DeleteWatchlist(string id, CancellationToken cancellationToken = default) => watchlistRepository.DeleteWatchlist(id, cancellationToken);

    public Task CreateWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default) => watchlistRepository.CreateWatchlistItem(dto, cancellationToken);
    public Task<bool> DeleteWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default) => watchlistRepository.DeleteWatchlistItem(dto, cancellationToken);
}