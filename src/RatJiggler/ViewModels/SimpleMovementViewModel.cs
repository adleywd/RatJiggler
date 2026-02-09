using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Data.Entities;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class SimpleMovementViewModel : ViewModelBase
{
    private readonly ILogger<SimpleMovementViewModel> _logger;
    private readonly INormalMouseService _normalMouseService;
    private readonly ISettingsService _settingsService;
    private readonly IStatusMessageService _statusMessageService;

    [ObservableProperty]
    private int _moveX = 50;

    [ObservableProperty]
    private int _moveY = 0;

    [ObservableProperty]
    private int _duration = 60;

    [ObservableProperty]
    private bool _backAndForth = true;

    [ObservableProperty]
    private bool _enableClick;

    [ObservableProperty]
    private int _clickButton = 1;

    [ObservableProperty]
    private int _clickIntervalSeconds = 5;

    [ObservableProperty]
    private bool _enableUserInterventionDetection = true;

    [ObservableProperty]
    private bool _isRunning = false;

    public SimpleMovementViewModel(
        ILogger<SimpleMovementViewModel> logger,
        INormalMouseService normalMouseService,
        ISettingsService settingsService,
        IStatusMessageService statusMessageService)
    {
        _logger = logger;
        _normalMouseService = normalMouseService;
        _settingsService = settingsService;
        _statusMessageService = statusMessageService;

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var settings = _settingsService.GetSimpleMovementSettingsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            MoveX = settings.MoveX;
            MoveY = settings.MoveY;
            Duration = settings.Duration;
            BackAndForth = settings.BackAndForth;
            EnableClick = settings.EnableClick;
            ClickButton = settings.ClickButton;
            ClickIntervalSeconds = settings.ClickIntervalSeconds;
            EnableUserInterventionDetection = settings.EnableUserInterventionDetection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading simple movement settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName != nameof(IsRunning) && IsRunning)
        {
            StopMovement();
        }
    }

    [RelayCommand]
    private void StartMovement()
    {
        try
        {
            _normalMouseService.Start(MoveX, MoveY, Duration, BackAndForth, EnableClick, ClickButton, ClickIntervalSeconds, EnableUserInterventionDetection, () => Dispatcher.UIThread.InvokeAsync(StopMovement));
            _statusMessageService.SetStatusMessage("Simple mouse movement started", "Green");
            IsRunning = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting simple movement");
            _statusMessageService.SetStatusMessage("Error starting simple movement", "Red");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            _normalMouseService.Stop();
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
            var settings = new SimpleMovementSettings
            {
                MoveX = MoveX,
                MoveY = MoveY,
                Duration = Duration,
                BackAndForth = BackAndForth,
                EnableClick = EnableClick,
                ClickButton = ClickButton,
                ClickIntervalSeconds = ClickIntervalSeconds,
                EnableUserInterventionDetection = EnableUserInterventionDetection
            };

            await _settingsService.SaveSimpleMovementSettingsAsync(settings).ConfigureAwait(false);
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
            await _settingsService.RestoreSimpleMovementDefaultsAsync().ConfigureAwait(false);
            LoadSettings();
            _statusMessageService.SetStatusMessage("Simple settings reset to defaults", "Black");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting simple settings");
            _statusMessageService.SetStatusMessage("Error resetting simple settings", "Red");
        }
    }
} 