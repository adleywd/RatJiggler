using System;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class ScreenWindowService : IScreenWindowService
{
    private readonly Window _window;
    
    public ScreenWindowService(Window window)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
    }
    
    public async Task<Rectangle> GetScreenBoundsAsync()
    {
        return await Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Get the Screens instance associated with the window
            Screens screens = _window.Screens;

            // Get the primary screen
            Screen? screen = screens.Primary;

            if (screen is null)
            {
                throw new InvalidOperationException("Unable to determine the screen for the specified window.");
            }

            // Get the screen bounds in DIPs
            PixelRect bounds = screen.Bounds;

            // Convert bounds to pixels
            var scalingFactor = screen.Scaling;
            var x = (int)(bounds.X * scalingFactor);
            var y = (int)(bounds.Y * scalingFactor);
            var width = (int)(bounds.Width * scalingFactor);
            var height = (int)(bounds.Height * scalingFactor);

            return new Rectangle(x, y, width, height);
        });
    }
}