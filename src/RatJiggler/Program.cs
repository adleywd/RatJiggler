using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls;
using Lemon.Hosting.AvaloniauiDesktop;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;
using RatJiggler.ViewModels;
using RatJiggler.Views;

namespace RatJiggler;

internal static class Program
    {
        [STAThread]
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        public static void Main(string[] args)
        {
            HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder();

            hostBuilder.Configuration
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddInMemoryCollection();

            hostBuilder.Services.AddLogging(builder => builder.AddConsole());
            
            hostBuilder.Services.AddSingleton<IMouseService, MouseService>();
            hostBuilder.Services.AddSingleton<IWindowService>(sp => new WindowService(new Window()));
            
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
            hostBuilder.Services.AddSingleton<MainWindowViewModel>();
            IHost appHost = hostBuilder.Build();
            appHost.RunAvaloniauiApplication(args);
        }
        
        private static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder) =>
            appBuilder
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }