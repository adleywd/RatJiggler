using System;
using System.Threading.Tasks;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Data.Entities;
using RatJiggler.Services.Interfaces;
using RatJiggler.Views;

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
    private string _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.0.0";

    [ObservableProperty]
    private WindowState _windowState;
    
    [ObservableProperty]
    private bool _showInTaskbar = true;
    
    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private bool _autoStartMovement;

    [ObservableProperty]
    private bool _isMovementRunning;
    
    [ObservableProperty]
    private bool _minimizeToTray;
    
    [ObservableProperty]
    private bool _startMinimizedToTray;
    
    public string Title => $"RatJiggler v{Version}";

    public string StatusMessageBackground => StatusMessageColor switch
    {
        "Red" => "#441111",
        "Green" => "#114411",
        "Yellow" => "#444411",
        _ => "#33225A"  // Default for Purple
    };
    public SimpleMovementViewModel SimpleMovementViewModel { get; }
    public RealisticMovementViewModel RealisticMovementViewModel { get; }

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
        ISettingsService settingsService,
        IStatusMessageService statusMessageService,
        SimpleMovementViewModel simpleMovementViewModel,
        RealisticMovementViewModel realisticMovementViewModel)
    {
        _logger = logger;
        _settingsService = settingsService;
        _statusMessageService = statusMessageService;

        _statusMessageService.StatusMessageChanged += OnStatusMessageChanged;

        SimpleMovementViewModel = simpleMovementViewModel;
        RealisticMovementViewModel = realisticMovementViewModel;

        LoadSettings();

        if (AutoStartMovement)
        {
            StartMouseMovementByHotkeyCommand.Execute(null);
        }
    }
    
        
    partial void OnWindowStateChanged(WindowState value)
    {
        if (!MinimizeToTray)
        {
            ShowInTaskbar = true;
            return;
        }
        ShowInTaskbar = value != WindowState.Minimized;
    }
    
    partial void OnMinimizeToTrayChanged(bool value)
    {
        if (value == false)
        {
            StartMinimizedToTray = false;
        }
        
        SaveSettings();
    }

    partial void OnStartMinimizedToTrayChanged(bool value)
    {
        SaveSettings();
    }
    
    partial void OnAutoStartMovementChanged(bool value)
    {
        SaveSettings();
    }
    
    private void OnStatusMessageChanged(object? sender, StatusMessageEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            StatusMessage = e.Message;
            StatusMessageColor = e.Color;
            OnPropertyChanged(nameof(StatusMessageBackground));
        });
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        // Stop any running movements when changing tabs
        if (value == 0) // Switched to Simple movement tab
        {
            if (RealisticMovementViewModel.IsRunning)
            {
                RealisticMovementViewModel.StopMovementCommand.Execute(null);
            }
            IsMovementRunning = SimpleMovementViewModel.IsRunning;
        }
        else // Switched to Realistic movement tab
        {
            if (SimpleMovementViewModel.IsRunning)
            {
                SimpleMovementViewModel.StopMovementCommand.Execute(null);
            }
            IsMovementRunning = RealisticMovementViewModel.IsRunning;
        }
        
        SaveSettings();
    }

    [RelayCommand]
    private void StartMouseMovementByHotkey()
    {
        try
        {
            if (SelectedTabIndex == 0)
            {
                SimpleMovementViewModel.StartMovementCommand.Execute(null);
                IsMovementRunning = SimpleMovementViewModel.IsRunning;
            }
            else
            {
                RealisticMovementViewModel.StartMovementCommand.Execute(null);
                IsMovementRunning = RealisticMovementViewModel.IsRunning;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing start movement hotkey");
            _statusMessageService.SetStatusMessage("Error starting movement", "Red");
        }
    }

    [RelayCommand]
    private void StopMovement()
    {
        try
        {
            if (SelectedTabIndex == 0)
            {
                SimpleMovementViewModel.StopMovementCommand.Execute(null);
                IsMovementRunning = SimpleMovementViewModel.IsRunning;
            }
            else
            {
                RealisticMovementViewModel.StopMovementCommand.Execute(null);
                IsMovementRunning = RealisticMovementViewModel.IsRunning;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing stop movement hotkey");
            _statusMessageService.SetStatusMessage("Error stopping movement", "Red");
        }
    }
    
    private void SaveSettings()
    {
        Task.Run(async () =>
        {
            try
            {
                var appSettings = new ApplicationSettings
                {
                    SelectedTabIndex = SelectedTabIndex,
                    AutoStartMovement = AutoStartMovement,
                    MinimizeToTray = MinimizeToTray,
                    StartMinimizedToTray = StartMinimizedToTray
                };
                await _settingsService.SaveApplicationSettingsAsync(appSettings).ConfigureAwait(false);
                
                _logger.LogInformation("Saving settings: {@Settings}", appSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving auto start movement setting");
                _statusMessageService.SetStatusMessage("Error saving settings", "Red");
            }
        });
    }
    
    private void LoadSettings()
    {
        try
        {
            var appSettings = _settingsService.GetApplicationSettingsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            SelectedTabIndex = appSettings.SelectedTabIndex;
            AutoStartMovement = appSettings.AutoStartMovement;
            MinimizeToTray = appSettings.MinimizeToTray;
            StartMinimizedToTray = appSettings.StartMinimizedToTray;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading application settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }
}