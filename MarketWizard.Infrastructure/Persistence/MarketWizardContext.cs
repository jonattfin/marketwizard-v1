using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MarketWizardContext : DbContext
{
    public MarketWizardContext(DbContextOptions<MarketWizardContext> options) : base(options)
    {
    }

    public DbSet<CronJob> CronJobs => Set<CronJob>();
}