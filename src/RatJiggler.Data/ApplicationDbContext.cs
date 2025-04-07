using Microsoft.EntityFrameworkCore;
using RatJiggler.Data.Entities;

namespace RatJiggler.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserSettingsEntity> UserSettings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserSettings entity
        modelBuilder.Entity<UserSettingsEntity>()
            .HasKey(u => u.Id);

        // Set default values for UserSettings
        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.MoveX)
            .HasDefaultValue(50);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.MoveY)
            .HasDefaultValue(0);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.Duration)
            .HasDefaultValue(60);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.BackForth)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.MinSpeed)
            .HasDefaultValue(3);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.MaxSpeed)
            .HasDefaultValue(7);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.EnableStepPauses)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.StepPauseMin)
            .HasDefaultValue(20);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.StepPauseMax)
            .HasDefaultValue(50);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.EnableRandomPauses)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.RandomPauseProbability)
            .HasDefaultValue(10);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.RandomPauseMin)
            .HasDefaultValue(100);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.RandomPauseMax)
            .HasDefaultValue(500);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.HorizontalBias)
            .HasDefaultValue(0f);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.VerticalBias)
            .HasDefaultValue(0f);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.PaddingPercentage)
            .HasDefaultValue(0.1f);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.SelectedMouseMovementModeIndex)
            .HasDefaultValue(0);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.EnableUserInterventionDetection)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserSettingsEntity>()
            .Property(u => u.MovementThresholdInPixels)
            .HasDefaultValue(10);
    }
} 