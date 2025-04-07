﻿using System;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls;
using Lemon.Hosting.AvaloniauiDesktop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatJiggler.Data;
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;
using RatJiggler.ViewModels;

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
        
        // Add database context
        hostBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseSqlite("Data Source=ratjiggler.data")
                .LogTo(Console.WriteLine, LogLevel.Information));

        // Add user settings service
        hostBuilder.Services.AddSingleton<IUserSettingsService, UserSettingsService>();
        
        AddPlatformSpecifServices(hostBuilder.Services);
        
        // Add services for the screen window, which is used to get the screen bounds
        hostBuilder.Services.AddSingleton<IScreenWindowService>(_ => new ScreenWindowService(new Window()));
        
        hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
        hostBuilder.Services.AddSingleton<MainWindowViewModel>();
        
        IHost appHost = hostBuilder.Build();

        // Initialize database
        using (var scope = appHost.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        appHost.RunAvaloniauiApplication(args);
    }

    private static void AddPlatformSpecifServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 0))
        {
            services.AddSingleton<IMouseService, WindowsMouseService>();
        }
        else if (OperatingSystem.IsLinux())
        {
            services.AddSingleton<IMouseService, LinuxMouseService>();
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system.");
        }
    }

    private static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder) =>
        appBuilder.UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}