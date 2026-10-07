using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Interfaces;

public interface IWatchlistRepository
{
    Task<List<WatchlistType>> GetAll(CancellationToken cancellationToken = default);

    Task<PagedResult<WatchlistType>> GetPaged(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default);

    Task<WatchlistType> CreateWatchlist(CreateWatchlistDto? dto = null, CancellationToken cancellationToken = default);

    Task<WatchlistType?> UpdateWatchlist(string id, UpdateWatchlistDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteWatchlist(string id, CancellationToken cancellationToken = default);
    Task CreateWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteWatchlistItem(WatchlistItemDto dto, CancellationToken cancellationToken = default);
}