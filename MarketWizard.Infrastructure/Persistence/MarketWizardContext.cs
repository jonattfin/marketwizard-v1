using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MarketWizardContext : DbContext
{
    public DbSet<CronJob> CronJobs { get; set; }
}