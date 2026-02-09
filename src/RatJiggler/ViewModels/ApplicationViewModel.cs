using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using RatJiggler.Views;

namespace RatJiggler.ViewModels;

public partial class ApplicationViewModel : ViewModelBase
{
    private readonly MainWindow _mainWindow;
    
    public ApplicationViewModel(Window? window)
    {
        if (window is MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        else
        {
            throw new ArgumentException("Window must be of type MainWindow");
        }
        
    }
    
    [RelayCommand]
    private void ShowWindow()
    {
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Show();

        // Center on screen — WindowStartupLocation only applies on first show,
        // so we need to manually center when restoring from tray.
        var screen = _mainWindow.Screens.Primary;
        if (screen != null)
        {
            var scaling = screen.Scaling;
            var x = (int)((screen.WorkingArea.Width - _mainWindow.Width * scaling) / 2) + screen.WorkingArea.X;
            var y = (int)((screen.WorkingArea.Height - _mainWindow.Height * scaling) / 2) + screen.WorkingArea.Y;
            _mainWindow.Position = new PixelPoint(x, y);
        }

        _mainWindow.BringIntoView();
        _mainWindow.Focus();
    }

    [RelayCommand]
    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            application.Shutdown();
        }
    }
}