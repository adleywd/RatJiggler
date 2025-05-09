using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RatJiggler.Data;
using RatJiggler.Data.Entities;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class SettingsService : ISettingsService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SettingsService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<SimpleMovementSettings> GetSimpleMovementSettingsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var settings = await dbContext.SimpleMovementSettings.FirstOrDefaultAsync();
        return settings ?? new SimpleMovementSettings { Id = 1 };
    }

    public async Task SaveSimpleMovementSettingsAsync(SimpleMovementSettings settings)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existingSettings = await dbContext.SimpleMovementSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            dbContext.SimpleMovementSettings.Add(settings);
        }
        else
        {
            existingSettings.MoveX = settings.MoveX;
            existingSettings.MoveY = settings.MoveY;
            existingSettings.Duration = settings.Duration;
            existingSettings.BackAndForth = settings.BackAndForth;
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task<RealisticMovementSettings> GetRealisticMovementSettingsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var settings = await dbContext.RealisticMovementSettings.FirstOrDefaultAsync();
        return settings ?? new RealisticMovementSettings { Id = 1 };
    }

    public async Task SaveRealisticMovementSettingsAsync(RealisticMovementSettings settings)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existingSettings = await dbContext.RealisticMovementSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            dbContext.RealisticMovementSettings.Add(settings);
        }
        else
        {
            existingSettings.MinSpeed = settings.MinSpeed;
            existingSettings.MaxSpeed = settings.MaxSpeed;
            existingSettings.EnableStepPauses = settings.EnableStepPauses;
            existingSettings.StepPauseMin = settings.StepPauseMin;
            existingSettings.StepPauseMax = settings.StepPauseMax;
            existingSettings.EnableRandomPauses = settings.EnableRandomPauses;
            existingSettings.RandomPauseProbability = settings.RandomPauseProbability;
            existingSettings.RandomPauseMin = settings.RandomPauseMin;
            existingSettings.RandomPauseMax = settings.RandomPauseMax;
            existingSettings.HorizontalBias = settings.HorizontalBias;
            existingSettings.VerticalBias = settings.VerticalBias;
            existingSettings.PaddingPercentage = settings.PaddingPercentage;
            existingSettings.RandomSeed = settings.RandomSeed;
            existingSettings.EnableUserInterventionDetection = settings.EnableUserInterventionDetection;
            existingSettings.MovementThresholdInPixels = settings.MovementThresholdInPixels;
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task<ApplicationSettings> GetApplicationSettingsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var settings = await dbContext.ApplicationSettings.FirstOrDefaultAsync();
        return settings ?? new ApplicationSettings { Id = 1 };
    }

    public async Task SaveApplicationSettingsAsync(ApplicationSettings settings)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existingSettings = await dbContext.ApplicationSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            dbContext.ApplicationSettings.Add(settings);
        }
        else
        {
            existingSettings.SelectedTabIndex = settings.SelectedTabIndex;
            existingSettings.AutoStartMovement = settings.AutoStartMovement;
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task RestoreSimpleMovementDefaultsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Remove existing settings
        dbContext.SimpleMovementSettings.RemoveRange(dbContext.SimpleMovementSettings);
        
        // Add default settings
        await dbContext.SimpleMovementSettings.AddAsync(new SimpleMovementSettings { Id = 1 });
        
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RestoreRealisticMovementDefaultsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Remove existing settings
        dbContext.RealisticMovementSettings.RemoveRange(dbContext.RealisticMovementSettings);
        
        // Add default settings
        await dbContext.RealisticMovementSettings.AddAsync(new RealisticMovementSettings { Id = 1 });
        
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RestoreDefaultsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Clear tables
        dbContext.SimpleMovementSettings.RemoveRange(dbContext.SimpleMovementSettings);
        dbContext.RealisticMovementSettings.RemoveRange(dbContext.RealisticMovementSettings);
        dbContext.ApplicationSettings.RemoveRange(dbContext.ApplicationSettings);
        
        await dbContext.SimpleMovementSettings.AddAsync(new SimpleMovementSettings { Id = 1 });
        await dbContext.RealisticMovementSettings.AddAsync(new RealisticMovementSettings { Id = 1 });
        await dbContext.ApplicationSettings.AddAsync(new ApplicationSettings { Id = 1 });
        
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
} 