using System;

namespace RatJiggler.Services.Interfaces;

public interface IGlobalHotkeyService : IDisposable
{
    event EventHandler? HotkeyPressed;
    void Register(int modifiers, int key);
    void Unregister();
    string GetDisplayString(int modifiers, int key);
}
