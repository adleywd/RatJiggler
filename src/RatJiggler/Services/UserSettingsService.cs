using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RatJiggler.Data;
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
        var settings =  _context.UserSettings.FirstOrDefault();
        
        if (settings != null)
        {
            return settings;
        }

        settings = new UserSettings();
        _context.UserSettings.Add(settings);
        _context.SaveChanges();
        return settings;
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        var existingSettings = await _context.UserSettings.FirstOrDefaultAsync().ConfigureAwait(ConfigureAwaitOptions.None);
        if (existingSettings == null)
        {
            _context.UserSettings.Add(settings);
        }
        else
        {
            settings.Id = existingSettings?.Id ?? 0;
            _context.Entry(existingSettings).CurrentValues.SetValues(settings);
            _context.Entry(existingSettings).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync().ConfigureAwait(ConfigureAwaitOptions.None);
    }
} 