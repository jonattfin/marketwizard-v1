using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using MediatR;

namespace MarketWizard.Application.Features.Watchlists.Commands;

public record CreateWatchlistCommand(CreateWatchlistDto? Dto = null) : IRequest<WatchlistType>;
public record UpdateWatchlistCommand(string Id, UpdateWatchlistDto Dto) : IRequest<WatchlistType?>;
public record DeleteWatchlistCommand(string Id) : IRequest<bool>;
public record CreateWatchlistItemCommand(WatchlistItemDto Dto) : IRequest<Unit>;
public record DeleteWatchlistItemCommand(WatchlistItemDto Dto) : IRequest<bool>;

public class WatchlistCommandHandlers(IWatchlistRepository watchlistRepository) :
    IRequestHandler<CreateWatchlistCommand, WatchlistType>,
    IRequestHandler<UpdateWatchlistCommand, WatchlistType?>,
    IRequestHandler<DeleteWatchlistCommand, bool>,
    IRequestHandler<CreateWatchlistItemCommand, Unit>,
    IRequestHandler<DeleteWatchlistItemCommand, bool>
{
    public Task<WatchlistType> Handle(CreateWatchlistCommand request, CancellationToken cancellationToken) =>
        watchlistRepository.CreateWatchlist(request.Dto, cancellationToken);

    public Task<WatchlistType?> Handle(UpdateWatchlistCommand request, CancellationToken cancellationToken) =>
        watchlistRepository.UpdateWatchlist(request.Id, request.Dto, cancellationToken);

    public Task<bool> Handle(DeleteWatchlistCommand request, CancellationToken cancellationToken) =>
        watchlistRepository.DeleteWatchlist(request.Id, cancellationToken);

    public async Task<Unit> Handle(CreateWatchlistItemCommand request, CancellationToken cancellationToken)
    {
        await watchlistRepository.CreateWatchlistItem(request.Dto, cancellationToken);
        return Unit.Value;
    }

    public Task<bool> Handle(DeleteWatchlistItemCommand request, CancellationToken cancellationToken) =>
        watchlistRepository.DeleteWatchlistItem(request.Dto, cancellationToken);
}
