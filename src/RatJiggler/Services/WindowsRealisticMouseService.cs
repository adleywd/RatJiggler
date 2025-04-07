using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

[SupportedOSPlatform("windows5.0")]
public class WindowsRealisticMouseService : IRealisticMouseService
{
    private readonly ILogger<WindowsRealisticMouseService> _logger;
    private CancellationTokenSource? _cts;
    private Task? _backgroundTask;

    public WindowsRealisticMouseService(ILogger<WindowsRealisticMouseService> logger)
    {
        _logger = logger;
    }

    public void StartRealistic(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null)
    {
        if (_backgroundTask is { IsCompleted: false })
        {
            _logger.LogInformation("Background task is already running.");
            return;
        }

        Console.WriteLine("Starting realistic background task...");
        _cts = new CancellationTokenSource();
        _backgroundTask = Task.Run(() => DoMoveRealisticAsync(mouseRealisticMovementDto, onStopped, _cts.Token));
    }

    public void Stop()
    {
        if (_cts == null || _cts.IsCancellationRequested)
        {
            return;
        }

        _logger.LogInformation("Stopping background task...");
        _cts.Cancel();
    }

    private async Task DoMoveRealisticAsync(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null, CancellationToken cancellationToken = default)
    {
        try
        {
            MouseUtility.MoveRealistic(
                mouseRealisticMovementDto,
                onStopped,
                cancellationToken);

            _logger.LogInformation("Realistic background task stopped.");
        }
        catch (OperationCanceledException)
        {
            // Task was canceled, ignore
            _logger.LogInformation("Realistic background task canceled.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in realistic background task");
            throw;
        }
    }
} 