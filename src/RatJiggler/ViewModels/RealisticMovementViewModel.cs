using System;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Models;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class RealisticMovementViewModel : ViewModelBase
{
    private readonly ILogger<RealisticMovementViewModel> _logger;
    private readonly IRealisticMouseService _realisticMouseService;
    private readonly IUserSettingsService _userSettingsService;
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

    public RealisticMovementViewModel(
        ILogger<RealisticMovementViewModel> logger,
        IRealisticMouseService realisticMouseService,
        IUserSettingsService userSettingsService,
        IScreenWindowService screenWindowService,
        IStatusMessageService statusMessageService)
    {
        _logger = logger;
        _realisticMouseService = realisticMouseService;
        _userSettingsService = userSettingsService;
        _screenWindowService = screenWindowService;
        _statusMessageService = statusMessageService;

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var settings = _userSettingsService.GetSettings();
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
            _logger.LogError(ex, "Error loading settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }

    [RelayCommand]
    private async Task StartMovement()
    {
        try
        {
            var screenBounds = await _screenWindowService.GetScreenBoundsAsync();
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
        MinSpeed = 3;
        MaxSpeed = 7;
        EnableStepPauses = true;
        StepPauseMin = 20;
        StepPauseMax = 50;
        EnableRandomPauses = true;
        RandomPauseProbability = 10;
        RandomPauseMin = 100;
        RandomPauseMax = 500;
        HorizontalBias = 0;
        VerticalBias = 0;
        PaddingPercentage = 0.1f;
        RandomSeed = null;
        EnableUserInterventionDetection = true;
        MovementThresholdInPixels = 10;
        _statusMessageService.SetStatusMessage("Settings reset to defaults", "Black");
    }
} 