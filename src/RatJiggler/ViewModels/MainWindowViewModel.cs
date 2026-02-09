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

namespace RatJiggler.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IStatusMessageService _statusMessageService;
    private readonly IGlobalHotkeyService _globalHotkeyService;

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

    [ObservableProperty]
    private int _hotkeyModifiers = 3;  // KeyModifiers.Control | KeyModifiers.Shift

    [ObservableProperty]
    private int _hotkeyKey = 98;  // Key.F9

    [ObservableProperty]
    private string _globalHotkeyDisplayString = "Ctrl+Shift+F9";

    [ObservableProperty]
    private bool _isCapturingHotkey;

    public string Title => $"RatJiggler v{Version}";

    public string StatusMessageBackground => StatusMessageColor switch
    {
        "Red" => "#441111",
        "Green" => "#114411",
        "Yellow" => "#444411",
        _ => "#33225A"  // Default for Purple
    };

    public string ToggleButtonText => IsMovementRunning ? "Stop" : "Start";

    public SimpleMovementViewModel SimpleMovementViewModel { get; }
    public RealisticMovementViewModel RealisticMovementViewModel { get; }

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
        ISettingsService settingsService,
        IStatusMessageService statusMessageService,
        IGlobalHotkeyService globalHotkeyService,
        SimpleMovementViewModel simpleMovementViewModel,
        RealisticMovementViewModel realisticMovementViewModel)
    {
        _logger = logger;
        _settingsService = settingsService;
        _statusMessageService = statusMessageService;
        _globalHotkeyService = globalHotkeyService;

        _statusMessageService.StatusMessageChanged += OnStatusMessageChanged;
        _globalHotkeyService.HotkeyPressed += OnGlobalHotkeyPressed;

        SimpleMovementViewModel = simpleMovementViewModel;
        RealisticMovementViewModel = realisticMovementViewModel;

        SimpleMovementViewModel.PropertyChanged += OnChildViewModelPropertyChanged;
        RealisticMovementViewModel.PropertyChanged += OnChildViewModelPropertyChanged;

        LoadSettings();

        _globalHotkeyService.Register(HotkeyModifiers, HotkeyKey);

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

    private void OnChildViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SimpleMovementViewModel.IsRunning))
        {
            if (SelectedTabIndex == 0 && sender == SimpleMovementViewModel)
            {
                IsMovementRunning = SimpleMovementViewModel.IsRunning;
            }
            else if (SelectedTabIndex != 0 && sender == RealisticMovementViewModel)
            {
                IsMovementRunning = RealisticMovementViewModel.IsRunning;
            }
        }
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

    private void OnGlobalHotkeyPressed(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (IsMovementRunning)
            {
                StopMovementCommand.Execute(null);
            }
            else
            {
                StartMouseMovementByHotkeyCommand.Execute(null);
            }
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

    partial void OnIsMovementRunningChanged(bool value)
    {
        OnPropertyChanged(nameof(ToggleButtonText));
    }

    [RelayCommand]
    private void ToggleMovement()
    {
        if (IsMovementRunning)
        {
            StopMovementCommand.Execute(null);
        }
        else
        {
            StartMouseMovementByHotkeyCommand.Execute(null);
        }
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

    public void SetHotkey(int modifiers, int key)
    {
        _globalHotkeyService.Unregister();
        HotkeyModifiers = modifiers;
        HotkeyKey = key;
        GlobalHotkeyDisplayString = _globalHotkeyService.GetDisplayString(modifiers, key);
        IsCapturingHotkey = false;
        _globalHotkeyService.Register(HotkeyModifiers, HotkeyKey);
        SaveSettings();
    }

    [RelayCommand]
    private void StartCapturingHotkey()
    {
        IsCapturingHotkey = true;
    }

    [RelayCommand]
    private void CancelCapturingHotkey()
    {
        IsCapturingHotkey = false;
    }

    [RelayCommand]
    private void ResetHotkey()
    {
        SetHotkey(3, 98); // Ctrl+Shift+F9
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
                    StartMinimizedToTray = StartMinimizedToTray,
                    HotkeyModifiers = HotkeyModifiers,
                    HotkeyKey = HotkeyKey
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
            HotkeyModifiers = appSettings.HotkeyModifiers;
            HotkeyKey = appSettings.HotkeyKey;
            GlobalHotkeyDisplayString = _globalHotkeyService.GetDisplayString(HotkeyModifiers, HotkeyKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading application settings");
            _statusMessageService.SetStatusMessage("Error loading settings", "Red");
        }
    }
}
