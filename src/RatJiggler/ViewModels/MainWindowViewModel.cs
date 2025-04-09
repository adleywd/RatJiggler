﻿using System;
using System.Threading.Tasks;
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

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _statusMessageColor = "Purple";

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private bool _autoStartMovement;

    partial void OnAutoStartMovementChanged(bool value)
    {
        Task.Run(async () =>
        {
            try
            {
                var appSettings = new ApplicationSettings
                {
                    SelectedTabIndex = SelectedTabIndex,
                    AutoStartMovement = value
                };

                await _settingsService.SaveApplicationSettingsAsync(appSettings).ConfigureAwait(false);
                _logger.LogInformation("Auto start movement setting saved: {Value}", value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving auto start movement setting");
                _statusMessageService.SetStatusMessage("Error saving settings", "Red");
            }
        });
    }

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

        if (AutoStartMovement)
        {
            StartMouseMovementByHotkeyCommand.Execute(null);
        }
    }

    private void LoadSettings()
    {
        try
        {
            var appSettings = _settingsService.GetApplicationSettingsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            SelectedTabIndex = appSettings.SelectedTabIndex;
            AutoStartMovement = appSettings.AutoStartMovement;
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
        // Stop any running movements when changing tabs
        if (value == 0) // Switched to Normal movement tab
        {
            if (RealisticMovementViewModel.IsRunning)
            {
                RealisticMovementViewModel.StopMovementCommand.Execute(null);
            }
        }
        else // Switched to Realistic movement tab
        {
            if (NormalMovementViewModel.IsRunning)
            {
                NormalMovementViewModel.StopMovementCommand.Execute(null);
            }
        }
        
        SaveSelectedTabAsync();
    }

    private async void SaveSelectedTabAsync()
    {
        try
        {
            var appSettings = new ApplicationSettings
            {
                SelectedTabIndex = SelectedTabIndex,
                AutoStartMovement = AutoStartMovement
            };

            await _settingsService.SaveApplicationSettingsAsync(appSettings).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving selected tab");
            _statusMessageService.SetStatusMessage("Error saving selected tab", "Red");
        }
    }

    [RelayCommand]
    private void StartMouseMovementByHotkey()
    {
        try
        {
            if (SelectedTabIndex == 0)
            {
                NormalMovementViewModel.StartMovementCommand.Execute(null);
            }
            else
            {
                RealisticMovementViewModel.StartMovementCommand.Execute(null);
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
                NormalMovementViewModel.StopMovementCommand.Execute(null);
            }
            else
            {
                RealisticMovementViewModel.StopMovementCommand.Execute(null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing stop movement hotkey");
            _statusMessageService.SetStatusMessage("Error stopping movement", "Red");
        }
    }
}