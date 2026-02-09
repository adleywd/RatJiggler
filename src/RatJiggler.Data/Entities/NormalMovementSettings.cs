namespace RatJiggler.Data.Entities;

public class SimpleMovementSettings
{
    public int Id { get; set; }
    public int MoveX { get; set; } = 50;
    public int MoveY { get; set; }
    public int Duration { get; set; } = 60;
    public bool BackAndForth { get; set; } = true;
    public bool EnableClick { get; set; }
    public int ClickButton { get; set; } = 1;
    public int ClickIntervalSeconds { get; set; } = 5;
    public bool EnableUserInterventionDetection { get; set; } = true;
} 