namespace RatJiggler.Data.Entities;

public class ApplicationSettings
{
    public int Id { get; set; }
    public int SelectedTabIndex { get; set; }
    public bool AutoStartMovement { get; set; }

    public bool MinimizeToTray { get; set; }
    
    public bool StartMinimizedToTray { get; set; }
} 