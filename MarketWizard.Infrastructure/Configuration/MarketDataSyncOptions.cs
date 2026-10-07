namespace Infrastructure.Configuration;

public class MarketDataSyncOptions
{
    public const string SectionName = "MarketDataSync";

    public bool Enabled { get; set; } = true;
    public double IntervalHours { get; set; } = 24.0;
    public bool RunOnStartup { get; set; } = true;
    public int InitialDelaySeconds { get; set; } = 5;
}
