using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RatJiggler.Services.Interfaces;
using RatJiggler.ViewModels;
using RatJiggler.Views;

namespace RatJiggler;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider) : base()
    {
        _serviceProvider = serviceProvider;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
            DataContext = new ApplicationViewModel(desktop.MainWindow);

            desktop.ShutdownRequested += (_, _) =>
            {
                var hotkeyService = _serviceProvider.GetRequiredService<IGlobalHotkeyService>();
                hotkeyService.Dispose();
            };

            if (viewModel.MinimizeToTray && viewModel.StartMinimizedToTray)
            {
                desktop.MainWindow.WindowState = WindowState.Minimized;
                desktop.MainWindow.ShowInTaskbar = false;
                desktop.MainWindow.Show();
                desktop.MainWindow.Hide();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
