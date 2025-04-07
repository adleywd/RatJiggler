namespace RatJiggler.Data.Entities;

public class RealisticMovementSettings
{
    public int Id { get; set; }
    public int MinSpeed { get; set; } = 3;
    public int MaxSpeed { get; set; } = 7;
    public bool EnableStepPauses { get; set; } = true;
    public int StepPauseMin { get; set; } = 20;
    public int StepPauseMax { get; set; } = 50;
    public bool EnableRandomPauses { get; set; } = true;
    public int RandomPauseProbability { get; set; } = 10;
    public int RandomPauseMin { get; set; } = 100;
    public int RandomPauseMax { get; set; } = 500;
    public float HorizontalBias { get; set; } = 0;
    public float VerticalBias { get; set; } = 0;
    public float PaddingPercentage { get; set; } = 0.1f;
    public int? RandomSeed { get; set; } = null;
    public bool EnableUserInterventionDetection { get; set; } = true;
    public int MovementThresholdInPixels { get; set; } = 10;
} 