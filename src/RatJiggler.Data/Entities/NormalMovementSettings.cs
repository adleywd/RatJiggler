namespace RatJiggler.Data.Entities;

public class NormalMovementSettings
{
    public int Id { get; set; }
    public int MoveX { get; set; } = 50;
    public int MoveY { get; set; }
    public int Duration { get; set; } = 60;
    public bool BackAndForth { get; set; } = true;
} 