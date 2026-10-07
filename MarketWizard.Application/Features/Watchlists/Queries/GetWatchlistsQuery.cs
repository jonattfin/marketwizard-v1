using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using MediatR;

namespace MarketWizard.Application.Features.Watchlists.Queries;

public record GetWatchlistsQuery : IRequest<List<WatchlistType>>;

public class GetWatchlistsQueryHandler(IWatchlistRepository watchlistRepository)
    : IRequestHandler<GetWatchlistsQuery, List<WatchlistType>>
{
    public Task<List<WatchlistType>> Handle(GetWatchlistsQuery request, CancellationToken cancellationToken) =>
        watchlistRepository.GetAll(cancellationToken);
}
