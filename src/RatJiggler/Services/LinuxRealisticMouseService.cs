using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using RatJiggler.MouseUtilities.Linux;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class LinuxRealisticMouseService : IRealisticMouseService
{
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;
    private readonly Random _random = new();

    public LinuxRealisticMouseService()
    {
        LinuxMouseUtility.Initialize();
    }

    public void StartRealistic(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null)
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
            var lastPosition = new Point(0, 0);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Calculate next position with realistic movement
                    var nextPosition = CalculateNextPosition(lastPosition, mouseRealisticMovementDto);
                    
                    // Move mouse to new position
                    LinuxMouseUtility.MoveMouse(nextPosition.X, nextPosition.Y);
                    lastPosition = nextPosition;

                    // Add step pause if enabled
                    if (mouseRealisticMovementDto.EnableStepPauses)
                    {
                        var stepPause = _random.Next(
                            mouseRealisticMovementDto.StepPauseMin,
                            mouseRealisticMovementDto.StepPauseMax);
                        Thread.Sleep(stepPause);
                    }

                    // Add random pause if enabled
                    if (mouseRealisticMovementDto.EnableRandomPauses &&
                        _random.Next(100) < mouseRealisticMovementDto.RandomPauseProbability)
                    {
                        var randomPause = _random.Next(
                            mouseRealisticMovementDto.RandomPauseMin,
                            mouseRealisticMovementDto.RandomPauseMax);
                        Thread.Sleep(randomPause);
                    }
                }
                catch (Exception)
                {
                    // Log error if needed
                    break;
                }
            }

            onStopped?.Invoke();
        }, token);
    }

    private Point CalculateNextPosition(
        Point currentPosition,
        MouseRealisticMovementDto settings)
    {
        var speed = _random.Next(settings.MinSpeed, settings.MaxSpeed + 1);
        var angle = _random.NextDouble() * 2 * Math.PI;

        // Apply horizontal and vertical bias
        angle += settings.HorizontalBias * Math.Cos(angle) + settings.VerticalBias * Math.Sin(angle);

        var dx = (int)(speed * Math.Cos(angle));
        var dy = (int)(speed * Math.Sin(angle));

        var newX = currentPosition.X + dx;
        var newY = currentPosition.Y + dy;

        // Apply padding to keep mouse within screen bounds
        var padding = (int)(settings.ScreenBounds.Width * settings.PaddingPercentage);
        newX = Math.Clamp(newX, padding, settings.ScreenBounds.Width - padding);
        newY = Math.Clamp(newY, padding, settings.ScreenBounds.Height - padding);

        return new Point(newX, newY);
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