using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Interfaces;

public interface IMarketDataProvider
{
    Task<IndicePerformanceData> GetIndicesAsync(CancellationToken cancellationToken = default);
    Task<TopNewsData> GetTopNewsAsync(CancellationToken cancellationToken = default);
    Task<SectorPerformanceData> GetSectorPerformanceAsync(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopGainersAsync(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopLosersAsync(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopIndustriesAsync(CancellationToken cancellationToken = default);
    Task<GainersData> GetWorstIndustriesAsync(CancellationToken cancellationToken = default);
}
