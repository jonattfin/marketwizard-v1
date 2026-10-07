using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using MediatR;

namespace MarketWizard.Application.Features.Watchlists.Queries;

public record GetWatchlistsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<WatchlistType>>;

public class GetWatchlistsQueryHandler(IWatchlistRepository watchlistRepository)
    : IRequestHandler<GetWatchlistsQuery, PagedResult<WatchlistType>>
{
    public Task<PagedResult<WatchlistType>> Handle(GetWatchlistsQuery request, CancellationToken cancellationToken) =>
        watchlistRepository.GetPaged(request.PageNumber, request.PageSize, cancellationToken);
}