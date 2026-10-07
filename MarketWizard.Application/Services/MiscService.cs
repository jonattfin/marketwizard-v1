using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Services;

public interface IMiscService
{
    Task<IndicePerformanceData> GetIndices(CancellationToken cancellationToken = default);
    Task<TopNewsData> GetTopNews(CancellationToken cancellationToken = default);
    Task<SectorPerformanceData> GetSectorPerformance(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopGainers(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopLosers(CancellationToken cancellationToken = default);
    Task<GainersData> GetTopIndustries(CancellationToken cancellationToken = default);
    Task<GainersData> GetWorstIndustries(CancellationToken cancellationToken = default);
}

public class MiscService(IMiscRepository miscRepository) : IMiscService
{
    public Task<IndicePerformanceData> GetIndices(CancellationToken cancellationToken = default) => miscRepository.GetIndices(cancellationToken);
    public Task<TopNewsData> GetTopNews(CancellationToken cancellationToken = default) => miscRepository.GetTopNews(cancellationToken);
    public Task<SectorPerformanceData> GetSectorPerformance(CancellationToken cancellationToken = default) => miscRepository.GetSectorPerformance(cancellationToken);
    public Task<GainersData> GetTopGainers(CancellationToken cancellationToken = default) => miscRepository.GetTopGainers(cancellationToken);
    
    public Task<GainersData> GetTopLosers(CancellationToken cancellationToken = default) => miscRepository.GetTopLosers(cancellationToken);
    public Task<GainersData> GetTopIndustries(CancellationToken cancellationToken = default) => miscRepository.GetTopIndustries(cancellationToken);
    public Task<GainersData> GetWorstIndustries(CancellationToken cancellationToken = default) => miscRepository.GetWorstIndustries(cancellationToken);
}