using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RatJiggler.Data;
using RatJiggler.Data.Entities;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _dbContext;

    public SettingsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NormalMovementSettings> GetNormalMovementSettingsAsync()
    {
        var settings = await _dbContext.NormalMovementSettings.FirstOrDefaultAsync();
        return settings ?? new NormalMovementSettings { Id = 1 };
    }

    public async Task SaveNormalMovementSettingsAsync(NormalMovementSettings settings)
    {
        var existingSettings = await _dbContext.NormalMovementSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            _dbContext.NormalMovementSettings.Add(settings);
        }
        else
        {
            existingSettings.MoveX = settings.MoveX;
            existingSettings.MoveY = settings.MoveY;
            existingSettings.Duration = settings.Duration;
            existingSettings.BackAndForth = settings.BackAndForth;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RealisticMovementSettings> GetRealisticMovementSettingsAsync()
    {
        var settings = await _dbContext.RealisticMovementSettings.FirstOrDefaultAsync();
        return settings ?? new RealisticMovementSettings { Id = 1 };
    }

    public async Task SaveRealisticMovementSettingsAsync(RealisticMovementSettings settings)
    {
        var existingSettings = await _dbContext.RealisticMovementSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            _dbContext.RealisticMovementSettings.Add(settings);
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
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ApplicationSettings> GetApplicationSettingsAsync()
    {
        var settings = await _dbContext.ApplicationSettings.FirstOrDefaultAsync();
        return settings ?? new ApplicationSettings { Id = 1 };
    }

    public async Task SaveApplicationSettingsAsync(ApplicationSettings settings)
    {
        var existingSettings = await _dbContext.ApplicationSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            settings.Id = 1;
            _dbContext.ApplicationSettings.Add(settings);
        }
        else
        {
            existingSettings.SelectedTabIndex = settings.SelectedTabIndex;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task RestoreDefaultsAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM NormalMovementSettings");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM RealisticMovementSettings");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM ApplicationSettings");
        
        await _dbContext.NormalMovementSettings.AddAsync(new NormalMovementSettings { Id = 1 });
        await _dbContext.RealisticMovementSettings.AddAsync(new RealisticMovementSettings { Id = 1 });
        await _dbContext.ApplicationSettings.AddAsync(new ApplicationSettings { Id = 1 });
        
        await _dbContext.SaveChangesAsync();
    }
} 