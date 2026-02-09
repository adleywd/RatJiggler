using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

[SupportedOSPlatform("windows5.0")]
public class WindowsNormalMouseService : INormalMouseService
{
    private readonly ILogger<WindowsNormalMouseService> _logger;
    private CancellationTokenSource? _cts;
    private Task? _backgroundTask;
    private Task? _clickTask;

    public WindowsNormalMouseService(ILogger<WindowsNormalMouseService> logger)
    {
        _logger = logger;
    }

    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement, bool enableClick, int clickButton, int clickIntervalSeconds = 5, bool enableUserInterventionDetection = true, Action? onStopped = null)
    {
        if (_backgroundTask is { IsCompleted: false })
        {
            Console.WriteLine("Background task is already running.");
            return;
        }

        _logger.LogInformation("Starting background task...");
        _cts = new CancellationTokenSource();
        _backgroundTask = Task.Run(() => DoMoveAsync(moveX, moveY, secondsBetweenMovement, backAndForthMovement, enableUserInterventionDetection, onStopped, _cts.Token));

        if (enableClick)
        {
            _clickTask = Task.Run(() => DoClickAsync(clickButton, clickIntervalSeconds, _cts.Token));
        }
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
        bool enableUserInterventionDetection,
        Action? onStopped,
        CancellationToken cancellationToken)
    {
        const int interventionThresholdPixels = 10;
        Point lastPosition = GetCursorPosition();

        while (!cancellationToken.IsCancellationRequested)
        {
            if (enableUserInterventionDetection)
            {
                var currentPosition = GetCursorPosition();
                var deltaX = currentPosition.X - lastPosition.X;
                var deltaY = currentPosition.Y - lastPosition.Y;
                var distanceMoved = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                if (distanceMoved > interventionThresholdPixels)
                {
                    _logger.LogInformation("User mouse intervention detected ({Distance:F1}px). Stopping simple movement.", distanceMoved);
                    onStopped?.Invoke();
                    return;
                }
            }

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

            lastPosition = GetCursorPosition();

            await Task.Delay(TimeSpan.FromSeconds(secondsBetweenMovement), cancellationToken).ConfigureAwait(false);
        }

        Console.WriteLine("Background task stopped.");
    }

    private async Task DoClickAsync(int clickButton, int clickIntervalSeconds, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(clickIntervalSeconds), cancellationToken).ConfigureAwait(false);
            MouseUtility.Click(clickButton);
        }
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    private static Point GetCursorPosition()
    {
        GetCursorPos(out var point);
        return new Point(point.X, point.Y);
    }
}
