using System;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Helpers;
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMouseService _mouseService;
    private readonly ILogger<MainWindowViewModel> _logger;

    public MainWindowViewModel()
    {
        AvaloniaUtilities.ThrowIfNotDesignMode();
#pragma warning disable CA1416
        _mouseService = new WindowsMouseService(
            new Logger<WindowsMouseService>(new LoggerFactory()),
            new ScreenWindowService(new Window()));
        _logger = new Logger<MainWindowViewModel>(new LoggerFactory());
#pragma warning restore CA1416
    }

    public MainWindowViewModel(IMouseService mouseService, ILogger<MainWindowViewModel> logger)
    {
        _mouseService = mouseService;
        _logger = logger;
    }

    [ObservableProperty] private string _statusMessage = "Stopped!";

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

    [ObservableProperty] private double _horizontalBias = 0;

    [ObservableProperty] private double _verticalBias = 0;

    [ObservableProperty] private double _paddingPercentage = 0.1;

    [RelayCommand]
    private void StartNormalMovement()
    {
        try
        {
            StatusMessage = "Running...";
            _mouseService.Start(MoveX, MoveY, Duration, BackForth);
        }
        catch (Exception ex)
        {
            StatusMessage = "Error... Stopped!";
            _logger.LogError(ex, "Error starting normal movement");
        }
    }

    [RelayCommand]
    private void StartRealisticMovement()
    {
        try
        {
            StatusMessage = "Running Realistic Movement...";
            _mouseService.StartRealistic(() => Dispatcher.UIThread.InvokeAsync(StopMovement));
        }
        catch (Exception ex)
        {
            StatusMessage = "Error...Stopped!";
            _logger.LogError(ex, "Error starting realistic movement");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            _mouseService.Stop();
            StatusMessage = "Stopped!";
        }
        catch (Exception ex)
        {
            StatusMessage = "Error while Stopping!";
            _logger.LogError(ex, "Error stopping movement, close the application to stop.");
        }
    }
}