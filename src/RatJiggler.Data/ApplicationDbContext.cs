using Microsoft.EntityFrameworkCore;
using RatJiggler.Data.Entities;

namespace RatJiggler.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<SimpleMovementSettings> SimpleMovementSettings { get; set; } = null!;
    public DbSet<RealisticMovementSettings> RealisticMovementSettings { get; set; } = null!;
    public DbSet<ApplicationSettings> ApplicationSettings { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial settings
        modelBuilder.Entity<SimpleMovementSettings>().HasData(
            new SimpleMovementSettings { Id = 1 }
        );

        modelBuilder.Entity<RealisticMovementSettings>().HasData(
            new RealisticMovementSettings { Id = 1 }
        );

        modelBuilder.Entity<ApplicationSettings>().HasData(
            new ApplicationSettings { Id = 1 }
        );
    }
} 