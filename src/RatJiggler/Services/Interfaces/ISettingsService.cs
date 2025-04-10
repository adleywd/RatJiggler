using System.Threading.Tasks;
using RatJiggler.Data.Entities;

namespace RatJiggler.Services.Interfaces;

public interface ISettingsService
{
    Task<SimpleMovementSettings> GetSimpleMovementSettingsAsync();
    Task SaveSimpleMovementSettingsAsync(SimpleMovementSettings settings);
    Task RestoreSimpleMovementDefaultsAsync();
    
    Task<RealisticMovementSettings> GetRealisticMovementSettingsAsync();
    Task SaveRealisticMovementSettingsAsync(RealisticMovementSettings settings);
    Task RestoreRealisticMovementDefaultsAsync();
    
    Task<ApplicationSettings> GetApplicationSettingsAsync();
    Task SaveApplicationSettingsAsync(ApplicationSettings settings);
    
    Task RestoreDefaultsAsync();
} 