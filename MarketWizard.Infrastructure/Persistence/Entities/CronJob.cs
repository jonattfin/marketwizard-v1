namespace Infrastructure.Persistence.Entities;

public class CronJob
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    
    public string? IndicePerfomance { get; set; }
    public string? TopNews { get; set; }
    public string? SectorPerformance { get; set; }
    
    public string? Gainers { get; set; }
    public string? Losers { get; set; }
    
    public string? TopIndustries { get; set; }
    public string? WorstIndustries { get; set; }
}