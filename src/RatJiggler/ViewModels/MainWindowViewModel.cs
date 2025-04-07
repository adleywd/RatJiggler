using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Models;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IStatusMessageService _statusMessageService;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _statusMessageColor = "Black";

    public NormalMovementViewModel NormalMovementViewModel { get; }
    public RealisticMovementViewModel RealisticMovementViewModel { get; }

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
        IUserSettingsService userSettingsService,
        IStatusMessageService statusMessageService,
        NormalMovementViewModel normalMovementViewModel,
        RealisticMovementViewModel realisticMovementViewModel)
    {
        _logger = logger;
        _userSettingsService = userSettingsService;
        _statusMessageService = statusMessageService;

        _statusMessageService.StatusMessageChanged += OnStatusMessageChanged;

        NormalMovementViewModel = normalMovementViewModel;
        RealisticMovementViewModel = realisticMovementViewModel;
    }

    private void OnStatusMessageChanged(object? sender, StatusMessageEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            StatusMessage = e.Message;
            StatusMessageColor = e.Color;
        });
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        try
        {
            var settings = new UserSettings
            {
                MoveX = 50,
                MoveY = 0,
                Duration = 60,
                BackAndForth = true,
                MinSpeed = 3,
                MaxSpeed = 7,
                EnableStepPauses = true,
                StepPauseMin = 20,
                StepPauseMax = 50,
                EnableRandomPauses = true,
                RandomPauseProbability = 10,
                RandomPauseMin = 100,
                RandomPauseMax = 500,
                HorizontalBias = 0,
                VerticalBias = 0,
                PaddingPercentage = 0.1f,
                RandomSeed = null,
                EnableUserInterventionDetection = true,
                MovementThresholdInPixels = 10
            };

            await _userSettingsService.SaveSettingsAsync(settings);
            _statusMessageService.SetStatusMessage("Settings saved successfully", "Green");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving settings");
            _statusMessageService.SetStatusMessage("Error saving settings", "Red");
        }
    }

    [RelayCommand]
    private async Task ResetToDefaults()
    {
        try
        {
            var settings = new UserSettings
            {
                MoveX = 50,
                MoveY = 0,
                Duration = 60,
                BackAndForth = true,
                MinSpeed = 3,
                MaxSpeed = 7,
                EnableStepPauses = true,
                StepPauseMin = 20,
                StepPauseMax = 50,
                EnableRandomPauses = true,
                RandomPauseProbability = 10,
                RandomPauseMin = 100,
                RandomPauseMax = 500,
                HorizontalBias = 0,
                VerticalBias = 0,
                PaddingPercentage = 0.1f,
                RandomSeed = null,
                EnableUserInterventionDetection = true,
                MovementThresholdInPixels = 10
            };

            await _userSettingsService.SaveSettingsAsync(settings).ConfigureAwait(false);
            _statusMessageService.SetStatusMessage("Settings reset to defaults", "Black");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting settings");
            _statusMessageService.SetStatusMessage("Error resetting settings", "Red");
        }
    }
}