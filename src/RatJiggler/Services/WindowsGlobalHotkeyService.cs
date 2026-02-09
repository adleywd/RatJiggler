using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Avalonia.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

[SupportedOSPlatform("windows5.0")]
public class WindowsGlobalHotkeyService : IGlobalHotkeyService
{
    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_ID = 1;

    // Win32 modifier constants
    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_WIN = 0x0008;
    private const uint MOD_NOREPEAT = 0x4000;

    private const uint WM_QUIT = 0x0012;

    private readonly ILogger<WindowsGlobalHotkeyService> _logger;
    private readonly ManualResetEventSlim _threadReady = new(false);
    private Thread? _messageThread;
    private uint _threadId;
    private bool _registered;
    private bool _disposed;

    public event EventHandler? HotkeyPressed;

    public WindowsGlobalHotkeyService(ILogger<WindowsGlobalHotkeyService> logger)
    {
        _logger = logger;
    }

    public void Register(int modifiers, int key)
    {
        Unregister();

        var winMods = TranslateModifiers((KeyModifiers)modifiers);
        var vk = TranslateKey((Key)key);

        if (vk == 0)
        {
            _logger.LogWarning("Could not translate key {Key} to a virtual key code", (Key)key);
            return;
        }

        var displayString = GetDisplayString(modifiers, key);

        _threadReady.Reset();
        _messageThread = new Thread(() => RunMessageLoop(winMods | MOD_NOREPEAT, vk, displayString))
        {
            IsBackground = true,
            Name = "GlobalHotkeyMessageLoop"
        };
        _messageThread.SetApartmentState(ApartmentState.STA);
        _messageThread.Start();

        // Wait for the thread to be ready (hotkey registered and message loop running)
        _threadReady.Wait(3000);
    }

    public void Unregister()
    {
        if (_registered && _threadId != 0)
        {
            PostThreadMessage(_threadId, WM_QUIT, 0, 0);
            _messageThread?.Join(3000);
        }

        _messageThread = null;
        _threadId = 0;
        _registered = false;
    }

    public string GetDisplayString(int modifiers, int key)
    {
        var mods = (KeyModifiers)modifiers;
        var k = (Key)key;
        var parts = new List<string>();
        if (mods.HasFlag(KeyModifiers.Control)) parts.Add("Ctrl");
        if (mods.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
        if (mods.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");
        if (mods.HasFlag(KeyModifiers.Meta)) parts.Add("Win");
        parts.Add(k.ToString());
        return string.Join("+", parts);
    }

    private void RunMessageLoop(uint winModifiers, uint vk, string displayString)
    {
        _threadId = GetCurrentThreadId();

        // RegisterHotKey with hwnd=0: WM_HOTKEY goes to the thread message queue directly.
        // No window class, no WndProc, no GC delegate issues.
        if (!RegisterHotKey(0, HOTKEY_ID, winModifiers, vk))
        {
            _logger.LogError("Failed to register global hotkey {Hotkey}. It may be in use by another application. Error: {Error}",
                displayString, Marshal.GetLastWin32Error());
            _threadReady.Set();
            return;
        }

        _registered = true;
        _logger.LogInformation("Global hotkey {Hotkey} registered successfully", displayString);
        _threadReady.Set();

        // GetMessage returns 0 on WM_QUIT, >0 on other messages, -1 on error
        while (GetMessage(out var msg, 0, 0, 0) > 0)
        {
            if (msg.message == WM_HOTKEY)
            {
                HotkeyPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        UnregisterHotKey(0, HOTKEY_ID);
        _registered = false;
        _logger.LogInformation("Global hotkey {Hotkey} unregistered", displayString);
    }

    private static uint TranslateModifiers(KeyModifiers modifiers)
    {
        uint result = 0;
        if (modifiers.HasFlag(KeyModifiers.Control)) result |= MOD_CONTROL;
        if (modifiers.HasFlag(KeyModifiers.Shift)) result |= MOD_SHIFT;
        if (modifiers.HasFlag(KeyModifiers.Alt)) result |= MOD_ALT;
        if (modifiers.HasFlag(KeyModifiers.Meta)) result |= MOD_WIN;
        return result;
    }

    private static uint TranslateKey(Key key)
    {
        return key switch
        {
            // Function keys
            >= Key.F1 and <= Key.F24 => (uint)(0x70 + (key - Key.F1)),
            // Letters
            >= Key.A and <= Key.Z => (uint)(0x41 + (key - Key.A)),
            // Digits
            >= Key.D0 and <= Key.D9 => (uint)(0x30 + (key - Key.D0)),
            // Numpad
            >= Key.NumPad0 and <= Key.NumPad9 => (uint)(0x60 + (key - Key.NumPad0)),
            // Special keys
            Key.Space => 0x20,
            Key.Enter => 0x0D,
            Key.Escape => 0x1B,
            Key.Tab => 0x09,
            Key.Back => 0x08,
            Key.Delete => 0x2E,
            Key.Insert => 0x2D,
            Key.Home => 0x24,
            Key.End => 0x23,
            Key.PageUp => 0x21,
            Key.PageDown => 0x22,
            Key.Up => 0x26,
            Key.Down => 0x28,
            Key.Left => 0x25,
            Key.Right => 0x27,
            Key.Pause => 0x13,
            Key.Scroll => 0x91,
            _ => 0
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            Unregister();
            _threadReady.Dispose();
        }
    }

    // P/Invoke declarations
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(nint hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(nint hWnd, int id);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetMessage(out MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool PostThreadMessage(uint idThread, uint msg, nint wParam, nint lParam);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

    [StructLayout(LayoutKind.Sequential)]
    private struct MSG
    {
        public nint hwnd;
        public uint message;
        public nint wParam;
        public nint lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }
}
