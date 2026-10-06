using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Interfaces;

public interface IMiscRepository
{
    Task<IndicePerformanceData> GetIndices();
    Task<TopNewsData> GetTopNews();
    Task<SectorPerformanceData> GetSectorPerformance();
    Task<GainersData> GetTopGainers();
    Task<GainersData> GetTopLosers();
    Task<GainersData> GetTopIndustries();
    Task<GainersData> GetWorstIndustries();
}