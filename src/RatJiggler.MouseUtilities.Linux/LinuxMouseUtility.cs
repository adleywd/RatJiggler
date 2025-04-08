using System;
using System.Runtime.InteropServices;

namespace RatJiggler.MouseUtilities.Linux;

public static class LinuxMouseUtility
{
    private const string X11 = "libX11.so.6";
    private const string Xtst = "libXtst.so.6";

    [DllImport(X11)]
    private static extern IntPtr XOpenDisplay(string display);

    [DllImport(X11)]
    private static extern int XCloseDisplay(IntPtr display);

    [DllImport(Xtst)]
    private static extern int XTestFakeButtonEvent(IntPtr display, int button, bool isPress, ulong delay);

    [DllImport(Xtst)]
    private static extern int XTestFakeMotionEvent(IntPtr display, int screen, int x, int y, ulong delay);

    [DllImport(X11)]
    private static extern int XFlush(IntPtr display);

    private static IntPtr? _display;

    public static void Initialize()
    {
        if (_display == null)
        {
            _display = XOpenDisplay(null);
            if (_display == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to open X11 display");
            }
        }
    }

    public static void Cleanup()
    {
        if (_display != null)
        {
            XCloseDisplay(_display.Value);
            _display = null;
        }
    }

    public static void MoveMouse(int x, int y)
    {
        if (_display == null)
        {
            throw new InvalidOperationException("X11 display not initialized");
        }

        XTestFakeMotionEvent(_display.Value, 0, x, y, 0);
        XFlush(_display.Value);
    }

    public static void MouseButtonDown(int button)
    {
        if (_display == null)
        {
            throw new InvalidOperationException("X11 display not initialized");
        }

        XTestFakeButtonEvent(_display.Value, button, true, 0);
        XFlush(_display.Value);
    }

    public static void MouseButtonUp(int button)
    {
        if (_display == null)
        {
            throw new InvalidOperationException("X11 display not initialized");
        }

        XTestFakeButtonEvent(_display.Value, button, false, 0);
        XFlush(_display.Value);
    }
} 