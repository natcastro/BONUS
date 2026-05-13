using Microsoft.EntityFrameworkCore;
using BonusTrackerApi.Models;

namespace BonusTrackerApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Appeal> Appeals => Set<Appeal>();
    public DbSet<UsaPeriodData> UsaPeriodData => Set<UsaPeriodData>();
    public DbSet<TikTokScore> TikTokScores => Set<TikTokScore>();
    public DbSet<MexAttendance> MexAttendances => Set<MexAttendance>();
    public DbSet<MexLiveSale> MexLiveSales => Set<MexLiveSale>();
    public DbSet<MexMonthlyGoal> MexMonthlyGoals => Set<MexMonthlyGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsaPeriodData>()
            .HasIndex(u => new { u.AgentId, u.Year, u.CycleId })
            .IsUnique();

        modelBuilder.Entity<MexAttendance>()
            .HasIndex(m => new { m.AgentId, m.Year, m.Month })
            .IsUnique();

        modelBuilder.Entity<MexMonthlyGoal>()
            .HasIndex(m => new { m.Year, m.Month })
            .IsUnique();

        // Seed default agents
        modelBuilder.Entity<Agent>().HasData(
            new Agent { Id = 1, Name = "Agent 1", Team = "USA" },
            new Agent { Id = 2, Name = "Agent 2", Team = "USA" },
            new Agent { Id = 3, Name = "Agent 1", Team = "MEX" },
            new Agent { Id = 4, Name = "Agent 2", Team = "MEX" }
        );
    }
}
