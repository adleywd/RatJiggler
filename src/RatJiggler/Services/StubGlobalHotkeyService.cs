using System;
using Avalonia.Input;
using Microsoft.Extensions.Logging;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class StubGlobalHotkeyService : IGlobalHotkeyService
{
    private readonly ILogger<StubGlobalHotkeyService> _logger;

#pragma warning disable CS0067 // Event is never used (stub implementation)
    public event EventHandler? HotkeyPressed;
#pragma warning restore CS0067

    public StubGlobalHotkeyService(ILogger<StubGlobalHotkeyService> logger)
    {
        _logger = logger;
    }

    public void Register(int modifiers, int key)
    {
        var displayString = GetDisplayString(modifiers, key);
        _logger.LogWarning("Global hotkeys are not supported on this platform. Hotkey {Hotkey} will not work globally.", displayString);
    }

    public void Unregister()
    {
    }

    public string GetDisplayString(int modifiers, int key)
    {
        var mods = (KeyModifiers)modifiers;
        var k = (Key)key;
        var parts = new System.Collections.Generic.List<string>();
        if (mods.HasFlag(KeyModifiers.Control)) parts.Add("Ctrl");
        if (mods.HasFlag(KeyModifiers.Shift)) parts.Add("Shift");
        if (mods.HasFlag(KeyModifiers.Alt)) parts.Add("Alt");
        if (mods.HasFlag(KeyModifiers.Meta)) parts.Add("Super");
        parts.Add(k.ToString());
        return string.Join("+", parts);
    }

    public void Dispose()
    {
    }
}
