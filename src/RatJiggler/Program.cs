using System;
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
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services;
using RatJiggler.Services.Interfaces;
using RatJiggler.ViewModels;
using RatJiggler.Views;

namespace RatJiggler;

internal static class Program
{
    private const string LocalDbConnectionString = "Data Source=RatJiggler.Data";

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
                .UseSqlite(LocalDbConnectionString)
#if DEBUG
                .LogTo(Console.WriteLine, LogLevel.Information));
#endif

        // Add settings service
        hostBuilder.Services.AddSingleton<ISettingsService, SettingsService>();

        // Add services for the screen window, which is used to get the screen bounds
        hostBuilder.Services.AddSingleton<IScreenWindowService>(_ => new ScreenWindowService(new Window()));
        
        // Add mouse services
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 0))
        {
            hostBuilder.Services.AddSingleton<INormalMouseService, WindowsNormalMouseService>();
            hostBuilder.Services.AddSingleton<IRealisticMouseService, WindowsRealisticMouseService>();
        }
        else if (OperatingSystem.IsLinux())
        {
            hostBuilder.Services.AddSingleton<INormalMouseService, LinuxMouseService>();
            hostBuilder.Services.AddSingleton<IRealisticMouseService, LinuxMouseService>();
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system.");
        }

        // Add status message service
        hostBuilder.Services.AddSingleton<IStatusMessageService, StatusMessageService>();

        // Add ViewModels
        hostBuilder.Services.AddTransient<NormalMovementViewModel>();
        hostBuilder.Services.AddTransient<RealisticMovementViewModel>();
        hostBuilder.Services.AddTransient<MainWindowViewModel>();

        // Add Views
        hostBuilder.Services.AddTransient<MainWindow>();
        hostBuilder.Services.AddTransient<NormalMovementView>();
        hostBuilder.Services.AddTransient<RealisticMovementView>();
        
        hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
        
        IHost appHost = hostBuilder.Build();
        
        // Initialize database
        using (var scope = appHost.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseStartup");
            try
            {
                logger.LogInformation("Applying database migrations...");
                dbContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
        
        appHost.RunAvaloniauiApplication(args);
    }

    private static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder) =>
        appBuilder.UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    // Used to display the app in design mode
    // public static AppBuilder BuildAvaloniaApp() =>
    //     AppBuilder.Configure<App>()
    //         .UsePlatformDetect()
    //         .LogToTrace();
}