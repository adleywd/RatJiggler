using System.Threading.Tasks;
using RatJiggler.Models;

namespace RatJiggler.Services.Interfaces;

public interface IUserSettingsService
{
    UserSettings GetSettings();
    Task SaveSettingsAsync(UserSettings settings);
}