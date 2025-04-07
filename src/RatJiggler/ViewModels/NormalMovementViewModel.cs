using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Models;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class NormalMovementViewModel : ViewModelBase
{
    private readonly ILogger<NormalMovementViewModel> _logger;
    private readonly INormalMouseService _normalMouseService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IStatusMessageService _statusMessageService;

    [ObservableProperty]
    private int _moveX = 50;

    [ObservableProperty]
    private int _moveY = 0;

    [ObservableProperty]
    private int _duration = 60;

    [ObservableProperty]
    private bool _backAndForth = true;

    public NormalMovementViewModel(
        ILogger<NormalMovementViewModel> logger,
        INormalMouseService normalMouseService,
        IUserSettingsService userSettingsService,
        IStatusMessageService statusMessageService)
    {
        _logger = logger;
        _normalMouseService = normalMouseService;
        _userSettingsService = userSettingsService;
        _statusMessageService = statusMessageService;

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var settings = _userSettingsService.GetSettings();
            MoveX = settings.MoveX;
            MoveY = settings.MoveY;
            Duration = settings.Duration;
            BackAndForth = settings.BackAndForth;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }

    [RelayCommand]
    private void StartMovement()
    {
        try
        {
            _normalMouseService.Start(MoveX, MoveY, Duration, BackAndForth);
            _statusMessageService.SetStatusMessage("Normal mouse movement started", "Green");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting normal movement");
            _statusMessageService.SetStatusMessage("Error starting normal movement", "Red");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            _normalMouseService.Stop();
            _statusMessageService.SetStatusMessage("Mouse movement stopped", "Black");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping movement");
            _statusMessageService.SetStatusMessage("Error stopping movement", "Red");
        }
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        try
        {
            var settings = new UserSettings
            {
                MoveX = MoveX,
                MoveY = MoveY,
                Duration = Duration,
                BackAndForth = BackAndForth
            };

            await _userSettingsService.SaveSettingsAsync(settings).ConfigureAwait(false);
            _statusMessageService.SetStatusMessage("Settings saved successfully", "Green");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving settings");
            _statusMessageService.SetStatusMessage("Error saving settings", "Red");
        }
    }

    [RelayCommand]
    private void ResetToDefaults()
    {
        MoveX = 50;
        MoveY = 0;
        Duration = 60;
        BackAndForth = true;
        _statusMessageService.SetStatusMessage("Settings reset to defaults", "Black");
    }
} 