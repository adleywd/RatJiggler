using System;

namespace RatJiggler.Helpers;

public static class AvaloniaUtilities
{
    public static void ThrowIfNotDesignMode()
    {
        if (!Avalonia.Controls.Design.IsDesignMode)
        {
            throw new InvalidOperationException("This method should only be used in design mode.");
        }
    }
}