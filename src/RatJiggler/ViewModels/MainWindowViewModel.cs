using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using RatJiggler.Data.Entities;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IStatusMessageService _statusMessageService;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _statusMessageColor = "Purple";

    [ObservableProperty]
    private int _selectedTabIndex;

    public NormalMovementViewModel NormalMovementViewModel { get; }
    public RealisticMovementViewModel RealisticMovementViewModel { get; }

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
        ISettingsService settingsService,
        IStatusMessageService statusMessageService,
        NormalMovementViewModel normalMovementViewModel,
        RealisticMovementViewModel realisticMovementViewModel)
    {
        _logger = logger;
        _settingsService = settingsService;
        _statusMessageService = statusMessageService;

        _statusMessageService.StatusMessageChanged += OnStatusMessageChanged;

        NormalMovementViewModel = normalMovementViewModel;
        RealisticMovementViewModel = realisticMovementViewModel;

        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var appSettings = _settingsService.GetApplicationSettingsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            SelectedTabIndex = appSettings.SelectedTabIndex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading application settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }

    private void OnStatusMessageChanged(object? sender, StatusMessageEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            StatusMessage = e.Message;
            StatusMessageColor = e.Color;
        });
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        SaveSelectedTabAsync();
    }

    private async void SaveSelectedTabAsync()
    {
        try
        {
            var appSettings = new ApplicationSettings
            {
                SelectedTabIndex = SelectedTabIndex
            };

            await _settingsService.SaveApplicationSettingsAsync(appSettings).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving selected tab");
            _statusMessageService.SetStatusMessage("Error saving selected tab", "Red");
        }
    }
}