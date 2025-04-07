using System;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Models;
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMouseService _mouseService;
    private readonly IScreenWindowService _screenWindowService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly ILogger<MainWindowViewModel> _logger;

    public MainWindowViewModel(
        IMouseService mouseService,
        IScreenWindowService screenWindowService,
        IUserSettingsService userSettingsService,
        ILogger<MainWindowViewModel> logger)
    {
        _mouseService = mouseService;
        _screenWindowService = screenWindowService;
        _userSettingsService = userSettingsService;
        _logger = logger;
        LoadSettings();
    }

    [ObservableProperty] private string _statusMessage = "Stopped!";

    [ObservableProperty] private IBrush _statusMessageColor = Brushes.MediumPurple;
    
    [ObservableProperty] private int _moveX = 50;

    [ObservableProperty] private int _moveY = 0;

    [ObservableProperty] private int _duration = 60;

    [ObservableProperty] private bool _backForth = true;

    [ObservableProperty] private int _minSpeed = 3;

    [ObservableProperty] private int _maxSpeed = 7;

    [ObservableProperty] private bool _enableStepPauses = true;

    [ObservableProperty] private int _stepPauseMin = 20;

    [ObservableProperty] private int _stepPauseMax = 50;

    [ObservableProperty] private bool _enableRandomPauses = true;

    [ObservableProperty] private int _randomPauseProbability = 10;

    [ObservableProperty] private int _randomPauseMin = 100;

    [ObservableProperty] private int _randomPauseMax = 500;

    [ObservableProperty] private float _horizontalBias = 0;

    [ObservableProperty] private float _verticalBias = 0;

    [ObservableProperty] private float _paddingPercentage = 0.1f;
    
    [ObservableProperty] private int _selectedMouseMovementModeIndex = 0;
    
    [ObservableProperty] private int? _randomSeed = null;
    
    [ObservableProperty] private bool _enableUserInterventionDetection = true;
    
    [ObservableProperty] private int _movementThresholdInPixels = 10;

    [RelayCommand]
    private async Task StartMouseMovementByHotkeyAsync()
    {
        switch (SelectedMouseMovementModeIndex)
        {
            case 0:
                await StartNormalMovementAsync().ConfigureAwait(ConfigureAwaitOptions.None);
                break;
            default:
                await StartRealisticMovementAsync().ConfigureAwait(ConfigureAwaitOptions.None);
                break;
        }
    }
    
    [RelayCommand]
    private async Task StartNormalMovementAsync()
    {
        try
        {
            await SaveSettingsAsync().ConfigureAwait(ConfigureAwaitOptions.None);
            SetStatusMessage(false);
            _mouseService.Start(MoveX, MoveY, Duration, BackForth);
        }
        catch (Exception ex)
        {
            SetStatusMessage(true, isError: true, message: "Error starting normal mouse movement");
            _logger.LogError(ex, "Error starting normal movement");
        }
    }

    [RelayCommand]
    private async Task StartRealisticMovementAsync()
    {
        try
        {
            await SaveSettingsAsync().ConfigureAwait(ConfigureAwaitOptions.None);
            SetStatusMessage(false);
            Rectangle screenBounds = await _screenWindowService.GetScreenBoundsAsync().ConfigureAwait(ConfigureAwaitOptions.None);
            var mouseRealisticMovementDto = new MouseRealisticMovementDto(
                screenBounds,
                MinSpeed,
                MaxSpeed,
                EnableStepPauses,
                StepPauseMin,
                StepPauseMax,
                EnableRandomPauses,
                RandomPauseProbability,
                RandomPauseMin,
                RandomPauseMax,
                RandomSeed, // Use the new property
                HorizontalBias,
                VerticalBias,
                PaddingPercentage,
                EnableUserInterventionDetection, // Use the new property
                MovementThresholdInPixels); // Use the new property
            _mouseService.StartRealistic(mouseRealisticMovementDto, () => Dispatcher.UIThread.InvokeAsync(StopMovement));
        }
        catch (Exception ex)
        {
            SetStatusMessage(true , isError: true, message: "Error starting realistic mouse movement");
            _logger.LogError(ex, "Error starting realistic movement");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            _mouseService.Stop();
            SetStatusMessage(true);
        }
        catch (Exception ex)
        {
            SetStatusMessage(true, isError: true, message: "Error while stopping mouse movement, close the application to stop.");
            _logger.LogError(ex, "Error stopping movement");
        }
    }

    private void SetStatusMessage(bool isStopped, bool isError = false, string? message = null)
    {
        if(isError && message is not null)
        {
            StatusMessage = message;
            StatusMessageColor = Brushes.OrangeRed;
            return;
        }
        
        switch (isStopped)
        {
            case true:
                StatusMessage = "Mouse movement stopped!";
                StatusMessageColor = Brushes.OrangeRed;
                break;
            default:
                StatusMessage = "Mouse movement started!";
                StatusMessageColor = Brushes.Green;
                break;
        }
    }
    
      private void LoadSettings()
    {
        try
        {
            var settings = _userSettingsService.GetSettings();
            MoveX = settings.MoveX;
            MoveY = settings.MoveY;
            Duration = settings.Duration;
            BackForth = settings.BackForth;
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
            SelectedMouseMovementModeIndex = settings.SelectedMouseMovementModeIndex;
            RandomSeed = settings.RandomSeed;
            EnableUserInterventionDetection = settings.EnableUserInterventionDetection;
            MovementThresholdInPixels = settings.MovementThresholdInPixels;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading settings");
        }
    }

    private async Task SaveSettingsAsync()
    {
        try
        {
            var settings = new UserSettings
            {
                Id = 1,
                MoveX = MoveX,
                MoveY = MoveY,
                Duration = Duration,
                BackForth = BackForth,
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
                SelectedMouseMovementModeIndex = SelectedMouseMovementModeIndex,
                RandomSeed = RandomSeed,
                EnableUserInterventionDetection = EnableUserInterventionDetection,
                MovementThresholdInPixels = MovementThresholdInPixels
            };
            await _userSettingsService.SaveSettingsAsync(settings).ConfigureAwait(ConfigureAwaitOptions.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving settings");
        }
    }
}