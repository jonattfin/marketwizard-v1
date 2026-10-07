using MarketWizard.Application.Interfaces;
using MediatR;

namespace MarketWizard.Application.Features.MarketDataSync.Commands;

public record SyncDailyMarketDataCommand : IRequest<Unit>;

public class SyncDailyMarketDataCommandHandler(IMarketDataSyncService syncService)
    : IRequestHandler<SyncDailyMarketDataCommand, Unit>
{
    public async Task<Unit> Handle(SyncDailyMarketDataCommand request, CancellationToken cancellationToken)
    {
        await syncService.SyncDailyMarketDataAsync(cancellationToken);
        return Unit.Value;
    }
}
