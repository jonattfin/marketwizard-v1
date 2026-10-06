using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Services;

public interface IMiscService
{
    Task<IndicePerformanceData> GetIndices();
    Task<TopNewsData> GetTopNews();
    Task<SectorPerformanceData> GetSectorPerformance();
    Task<GainersData> GetTopGainers();
    Task<GainersData> GetTopLosers();
    Task<GainersData> GetTopIndustries();
    Task<GainersData> GetWorstIndustries();
}

public class MiscService(IMiscRepository miscRepository) : IMiscService
{
    public Task<IndicePerformanceData> GetIndices() => miscRepository.GetIndices();
    public Task<TopNewsData> GetTopNews() => miscRepository.GetTopNews();
    public Task<SectorPerformanceData> GetSectorPerformance() => miscRepository.GetSectorPerformance();
    public Task<GainersData> GetTopGainers() => miscRepository.GetTopGainers();
    
    public Task<GainersData> GetTopLosers() => miscRepository.GetTopLosers();
    public Task<GainersData> GetTopIndustries() => miscRepository.GetTopIndustries();
    public Task<GainersData> GetWorstIndustries() => miscRepository.GetWorstIndustries();
}