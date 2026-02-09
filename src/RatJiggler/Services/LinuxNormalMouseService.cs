using System;
using System.Threading;
using System.Threading.Tasks;
using RatJiggler.MouseUtilities.Linux;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class LinuxNormalMouseService : INormalMouseService
{
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;

    public LinuxNormalMouseService()
    {
        LinuxMouseUtility.Initialize();
    }

    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement, bool enableClick, int clickButton, int clickIntervalSeconds = 5, bool enableUserInterventionDetection = true, Action? onStopped = null)
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        Task.Run(() =>
        {
            var direction = 1;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var currentX = moveX * direction;
                    var currentY = moveY * direction;

                    LinuxMouseUtility.MoveMouse(currentX, currentY);

                    if (backAndForthMovement)
                    {
                        direction *= -1;
                    }

                    Thread.Sleep(secondsBetweenMovement * 1000);
                }
                catch (Exception)
                {
                    // Log error if needed
                    break;
                }
            }
        }, token);

        if (enableClick)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(clickIntervalSeconds), token).ConfigureAwait(false);
                    LinuxMouseUtility.Click(clickButton);
                }
            }, token);
        }
    }

    public void Stop()
    {
        if (!_isRunning)
        {
            return;
        }

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _isRunning = false;
    }
} 