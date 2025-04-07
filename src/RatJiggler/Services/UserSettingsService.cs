using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RatJiggler.Data;
using RatJiggler.Data.Entities;
using RatJiggler.Models;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly ApplicationDbContext _context;

    public UserSettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public UserSettings GetSettings()
    {
        var entity = _context.UserSettings.FirstOrDefault();
        
        if (entity != null)
        {
            return MapToUserSettings(entity);
        }

        entity = new UserSettingsEntity();
        _context.UserSettings.Add(entity);
        _context.SaveChanges();
        return MapToUserSettings(entity);
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        var entity = await _context.UserSettings.FirstOrDefaultAsync();
        
        if (entity == null)
        {
            entity = new UserSettingsEntity();
            MapToEntity(settings, entity);
            _context.UserSettings.Add(entity);
        }
        else
        {
            MapToEntity(settings, entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        
        await _context.SaveChangesAsync();
    }

    private static UserSettings MapToUserSettings(UserSettingsEntity entity)
    {
        return new UserSettings
        {
            MoveX = entity.MoveX,
            MoveY = entity.MoveY,
            Duration = entity.Duration,
            BackForth = entity.BackForth,
            MinSpeed = entity.MinSpeed,
            MaxSpeed = entity.MaxSpeed,
            EnableStepPauses = entity.EnableStepPauses,
            StepPauseMin = entity.StepPauseMin,
            StepPauseMax = entity.StepPauseMax,
            EnableRandomPauses = entity.EnableRandomPauses,
            RandomPauseProbability = entity.RandomPauseProbability,
            RandomPauseMin = entity.RandomPauseMin,
            RandomPauseMax = entity.RandomPauseMax,
            HorizontalBias = entity.HorizontalBias,
            VerticalBias = entity.VerticalBias,
            PaddingPercentage = entity.PaddingPercentage,
            SelectedMouseMovementModeIndex = entity.SelectedMouseMovementModeIndex,
            RandomSeed = entity.RandomSeed,
            EnableUserInterventionDetection = entity.EnableUserInterventionDetection,
            MovementThresholdInPixels = entity.MovementThresholdInPixels
        };
    }

    private static void MapToEntity(UserSettings model, UserSettingsEntity entity)
    {
        entity.MoveX = model.MoveX;
        entity.MoveY = model.MoveY;
        entity.Duration = model.Duration;
        entity.BackForth = model.BackForth;
        entity.MinSpeed = model.MinSpeed;
        entity.MaxSpeed = model.MaxSpeed;
        entity.EnableStepPauses = model.EnableStepPauses;
        entity.StepPauseMin = model.StepPauseMin;
        entity.StepPauseMax = model.StepPauseMax;
        entity.EnableRandomPauses = model.EnableRandomPauses;
        entity.RandomPauseProbability = model.RandomPauseProbability;
        entity.RandomPauseMin = model.RandomPauseMin;
        entity.RandomPauseMax = model.RandomPauseMax;
        entity.HorizontalBias = model.HorizontalBias;
        entity.VerticalBias = model.VerticalBias;
        entity.PaddingPercentage = model.PaddingPercentage;
        entity.SelectedMouseMovementModeIndex = model.SelectedMouseMovementModeIndex;
        entity.RandomSeed = model.RandomSeed;
        entity.EnableUserInterventionDetection = model.EnableUserInterventionDetection;
        entity.MovementThresholdInPixels = model.MovementThresholdInPixels;
    }
} 