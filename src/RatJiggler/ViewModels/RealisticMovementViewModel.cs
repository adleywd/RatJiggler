using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Data.Entities;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class RealisticMovementViewModel : ViewModelBase
{
    private readonly ILogger<RealisticMovementViewModel> _logger;
    private readonly IRealisticMouseService _realisticMouseService;
    private readonly ISettingsService _settingsService;
    private readonly IScreenWindowService _screenWindowService;
    private readonly IStatusMessageService _statusMessageService;

    [ObservableProperty]
    private int _minSpeed = 3;

    [ObservableProperty]
    private int _maxSpeed = 7;

    [ObservableProperty]
    private bool _enableStepPauses = true;

    [ObservableProperty]
    private int _stepPauseMin = 20;

    [ObservableProperty]
    private int _stepPauseMax = 50;

    [ObservableProperty]
    private bool _enableRandomPauses = true;

    [ObservableProperty]
    private int _randomPauseProbability = 10;

    [ObservableProperty]
    private int _randomPauseMin = 100;

    [ObservableProperty]
    private int _randomPauseMax = 500;

    [ObservableProperty]
    private float _horizontalBias = 0;

    [ObservableProperty]
    private float _verticalBias = 0;

    [ObservableProperty]
    private float _paddingPercentage = 0.1f;

    [ObservableProperty]
    private int? _randomSeed = null;

    [ObservableProperty]
    private bool _enableUserInterventionDetection = true;

    [ObservableProperty]
    private int _movementThresholdInPixels = 10;

    [ObservableProperty]
    private bool _isRunning = false;

    public RealisticMovementViewModel(
        ILogger<RealisticMovementViewModel> logger,
        IRealisticMouseService realisticMouseService,
        ISettingsService settingsService,
        IScreenWindowService screenWindowService,
        IStatusMessageService statusMessageService)
    {
        _logger = logger;
        _realisticMouseService = realisticMouseService;
        _settingsService = settingsService;
        _screenWindowService = screenWindowService;
        _statusMessageService = statusMessageService;

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var settings = _settingsService.GetRealisticMovementSettingsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            MinSpeed = settings.MinSpeed;
            MaxSpeed = settings.MaxSpeed;
            EnableStepPauses = settings.EnableStepPauses;
            StepPauseMin = settings.StepPauseMin;
            StepPauseMax = settings.StepPauseMax;
            EnableRandomPauses = settings.EnableRandomPauses;
            RandomPauseProbability = settings.RandomPauseProbability;
            RandomPauseMin = settings.RandomPauseMin;
            RandomPauseMax = settings.RandomPauseMax;
            HorizontalBias = settings.HorizontalBias;
            VerticalBias = settings.VerticalBias;
            PaddingPercentage = settings.PaddingPercentage;
            RandomSeed = settings.RandomSeed;
            EnableUserInterventionDetection = settings.EnableUserInterventionDetection;
            MovementThresholdInPixels = settings.MovementThresholdInPixels;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading realistic movement settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }

    [RelayCommand]
    private async Task StartMovement()
    {
        try
        {
            var screenBounds = await _screenWindowService.GetScreenBoundsAsync().ConfigureAwait(false);
            var movementDto = new MouseRealisticMovementDto
            {
                ScreenBounds = screenBounds,
                MinSpeed = MinSpeed,
                MaxSpeed = MaxSpeed,
                EnableStepPauses = EnableStepPauses,
                StepPauseMin = StepPauseMin,
                StepPauseMax = StepPauseMax,
                EnableRandomPauses = EnableRandomPauses,
                RandomPauseProbability = RandomPauseProbability,
                RandomPauseMin = RandomPauseMin,
                RandomPauseMax = RandomPauseMax,
                RandomSeed = RandomSeed,
                HorizontalBias = HorizontalBias,
                VerticalBias = VerticalBias,
                PaddingPercentage = PaddingPercentage,
                EnableUserInterventionDetection = EnableUserInterventionDetection,
                MovementThresholdInPixels = MovementThresholdInPixels
            };

            _realisticMouseService.StartRealistic(movementDto, () => Dispatcher.UIThread.InvokeAsync(StopMovement));
            _statusMessageService.SetStatusMessage("Realistic mouse movement started", "Green");
            IsRunning = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting realistic movement");
            _statusMessageService.SetStatusMessage("Error starting realistic movement", "Red");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            _realisticMouseService.Stop();
            _statusMessageService.SetStatusMessage("Mouse movement stopped", "Red");
            IsRunning = false;
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
            var settings = new RealisticMovementSettings
            {
                MinSpeed = MinSpeed,
                MaxSpeed = MaxSpeed,
                EnableStepPauses = EnableStepPauses,
                StepPauseMin = StepPauseMin,
                StepPauseMax = StepPauseMax,
                EnableRandomPauses = EnableRandomPauses,
                RandomPauseProbability = RandomPauseProbability,
                RandomPauseMin = RandomPauseMin,
                RandomPauseMax = RandomPauseMax,
                HorizontalBias = HorizontalBias,
                VerticalBias = VerticalBias,
                PaddingPercentage = PaddingPercentage,
                RandomSeed = RandomSeed,
                EnableUserInterventionDetection = EnableUserInterventionDetection,
                MovementThresholdInPixels = MovementThresholdInPixels
            };

            await _settingsService.SaveRealisticMovementSettingsAsync(settings).ConfigureAwait(false);
            _statusMessageService.SetStatusMessage("Settings saved successfully", "Green");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving settings");
            _statusMessageService.SetStatusMessage("Error saving settings", "Red");
        }
    }

    [RelayCommand]
    private async Task RestoreDefaults()
    {
        try
        {
            await _settingsService.RestoreDefaultsAsync().ConfigureAwait(false);
            LoadSettings();
            _statusMessageService.SetStatusMessage("Settings reset to defaults", "Black");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting settings");
            _statusMessageService.SetStatusMessage("Error resetting settings", "Red");
        }
    }
} 