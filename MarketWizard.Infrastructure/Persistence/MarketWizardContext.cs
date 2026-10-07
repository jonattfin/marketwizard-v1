using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MarketWizardContext : DbContext
{
    public MarketWizardContext(DbContextOptions<MarketWizardContext> options) : base(options)
    {
    }

    public DbSet<CronJob> CronJobs => Set<CronJob>();
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Watchlist>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired();
            entity.HasMany(w => w.Items)
                .WithOne(i => i.Watchlist)
                .HasForeignKey(i => i.WatchlistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WatchlistItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired();
            entity.Property(i => i.Ticker).IsRequired();
        });
    }
}