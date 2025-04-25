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