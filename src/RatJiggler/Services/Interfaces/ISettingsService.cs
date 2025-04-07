using System.Threading.Tasks;
using RatJiggler.Data.Entities;

namespace RatJiggler.Services.Interfaces;

public interface ISettingsService
{
    Task<NormalMovementSettings> GetNormalMovementSettingsAsync();
    Task SaveNormalMovementSettingsAsync(NormalMovementSettings settings);
    
    Task<RealisticMovementSettings> GetRealisticMovementSettingsAsync();
    Task SaveRealisticMovementSettingsAsync(RealisticMovementSettings settings);
    
    Task<ApplicationSettings> GetApplicationSettingsAsync();
    Task SaveApplicationSettingsAsync(ApplicationSettings settings);
    
    Task RestoreDefaultsAsync();
} 