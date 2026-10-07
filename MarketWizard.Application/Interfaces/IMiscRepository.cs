using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Interfaces;

public interface IMiscRepository
{
    Task<IndicePerformanceData> GetIndices(CancellationToken cancellationToken = default);
    Task<TopNewsData> GetTopNews(CancellationToken cancellationToken = default);
    Task<SectorPerformanceData> GetSectorPerformance(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopGainers(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopLosers(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopIndustries(CancellationToken cancellationToken = default);
    Task<GainersData> GetWorstIndustries(CancellationToken cancellationToken = default);
}