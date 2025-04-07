using Microsoft.EntityFrameworkCore;
using RatJiggler.Models;

namespace RatJiggler.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserSettings> UserSettings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserSettings entity
        modelBuilder.Entity<UserSettings>()
            .HasKey(u => u.Id);

        // Set default values for UserSettings
        modelBuilder.Entity<UserSettings>()
            .Property(u => u.MoveX)
            .HasDefaultValue(50);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.MoveY)
            .HasDefaultValue(0);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.Duration)
            .HasDefaultValue(60);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.BackForth)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.MinSpeed)
            .HasDefaultValue(3);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.MaxSpeed)
            .HasDefaultValue(7);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.EnableStepPauses)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.StepPauseMin)
            .HasDefaultValue(20);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.StepPauseMax)
            .HasDefaultValue(50);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.EnableRandomPauses)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.RandomPauseProbability)
            .HasDefaultValue(10);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.RandomPauseMin)
            .HasDefaultValue(100);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.RandomPauseMax)
            .HasDefaultValue(500);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.HorizontalBias)
            .HasDefaultValue(0f);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.VerticalBias)
            .HasDefaultValue(0f);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.PaddingPercentage)
            .HasDefaultValue(0.1f);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.SelectedMouseMovementModeIndex)
            .HasDefaultValue(0);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.EnableUserInterventionDetection)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettings>()
            .Property(u => u.MovementThresholdInPixels)
            .HasDefaultValue(10);
    }
} 