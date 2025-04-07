using System;
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

    public WindowsNormalMouseService(ILogger<WindowsNormalMouseService> logger)
    {
        _logger = logger;
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
} 