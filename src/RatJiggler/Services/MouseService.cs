using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Microsoft.Extensions.Logging;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

[SupportedOSPlatform("windows5.0")]
public class MouseService : IMouseService
{
    private readonly ILogger<MouseService> _logger;
    private readonly IWindowService _windowService;
    private CancellationTokenSource? _cts;
    private Task? _backgroundTask;

    public MouseService(ILogger<MouseService> logger, IWindowService windowService)
    {
        _logger = logger;
        _windowService = windowService;
    }

    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement)
    {
        if (_backgroundTask is { IsCompleted: false })
        {
            Console.WriteLine("Background task is already running.");
            return;
        }

        _logger.LogInformation("Starting background task...");
        _cts = new CancellationTokenSource();
        _backgroundTask = Task.Run(() => DoMoveAsync(moveX, moveY, secondsBetweenMovement, backAndForthMovement, _cts.Token));
    }

    public void StartRealistic()
    {
        if (_backgroundTask is { IsCompleted: false })
        {
            _logger.LogInformation("Background task is already running.");
            return;
        }

        Console.WriteLine("Starting realistic background task...");
        _cts = new CancellationTokenSource();
        _backgroundTask = Task.Run(() => DoMoveRealisticAsync(_cts.Token));
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

    private async Task DoMoveAsync(
        int moveX,
        int moveY,
        int secondsBetweenMovement,
        bool backAndForthMovement,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (backAndForthMovement)
            {
                moveX *= -1;
                moveY *= -1;
            }

            _logger.LogInformation(
                "Moving mouse by X: {MoveX}, Y: {MoveY} every {SecondsBetweenMovement} seconds",
                moveX,
                moveY,
                secondsBetweenMovement);
            MouseUtility.Move(moveX, moveY);
            await Task.Delay(TimeSpan.FromSeconds(secondsBetweenMovement), cancellationToken).ConfigureAwait(false);
        }

        Console.WriteLine("Background task stopped.");
    }

    private async Task DoMoveRealisticAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Use the Dispatcher to marshal the call to the UI thread
            Rectangle screenBounds = await _windowService.GetScreenBoundsAsync();
            var mouseRealisticMovementDto = new MouseRealisticMovementDto(
                ScreenBounds: screenBounds,
                StatusMessage: "Running Realistic Movement...",
                MoveX: 0,
                MoveY: 0,
                Duration: 0,
                BackForth: false,
                MinSpeed: 3,
                MaxSpeed: 7,
                EnableStepPauses: true,
                StepPauseMin: 20,
                StepPauseMax: 50,
                EnableRandomPauses: true,
                RandomPauseProbability: 10,
                RandomPauseMin: 100,
                RandomPauseMax: 500,
                HorizontalBias: 0, // Favor horizontal movement
                VerticalBias: 0, // Slightly favor upward movement
                PaddingPercentage: 0.1f,
                RandomSeed: null); // Optional: for reproducible behavior

            MouseUtility.MoveRealistic(mouseRealisticMovementDto, cancellationToken);

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