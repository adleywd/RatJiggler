namespace RatJiggler.Data.Entities;

public class ApplicationSettings
{
    public int Id { get; set; }
    public int SelectedTabIndex { get; set; }
    public bool AutoStartMovement { get; set; }

    public bool MinimizeToTray { get; set; }
    
    public bool StartMinimizedToTray { get; set; }

    public int HotkeyModifiers { get; set; } = 3;  // KeyModifiers.Control | KeyModifiers.Shift
    public int HotkeyKey { get; set; } = 98;        // Key.F9
} 