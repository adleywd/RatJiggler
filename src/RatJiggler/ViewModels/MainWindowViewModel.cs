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
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMouseService _mouseService;
    private readonly IScreenWindowService _screenWindowService;
    private readonly ILogger<MainWindowViewModel> _logger;

    public MainWindowViewModel(IMouseService mouseService, IScreenWindowService screenWindowService, ILogger<MainWindowViewModel> logger)
    {
        _mouseService = mouseService;
        _screenWindowService = screenWindowService;
        _logger = logger;
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
                StartNormalMovement();
                break;
            default:
                await StartRealisticMovementAsync().ConfigureAwait(ConfigureAwaitOptions.None);
                break;
        }
    }
    
    [RelayCommand]
    private void StartNormalMovement()
    {
        try
        {
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
}