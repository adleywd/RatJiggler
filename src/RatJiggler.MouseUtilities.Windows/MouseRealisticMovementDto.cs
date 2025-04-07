using System.Drawing;

namespace RatJiggler.MouseUtilities.Windows;

public record MouseRealisticMovementDto
{
    public Rectangle ScreenBounds { get; init; }
    public int MinSpeed { get; init; }
    public int MaxSpeed { get; init; }
    public bool EnableStepPauses { get; init; }
    public int StepPauseMin { get; init; }
    public int StepPauseMax { get; init; }
    public bool EnableRandomPauses { get; init; }
    public int RandomPauseProbability { get; init; }
    public int RandomPauseMin { get; init; }
    public int RandomPauseMax { get; init; }
    public int? RandomSeed { get; init; }
    public float HorizontalBias { get; init; }
    public float VerticalBias { get; init; }
    public float PaddingPercentage { get; init; }
    public bool EnableUserInterventionDetection { get; init; }
    public int MovementThresholdInPixels { get; init; }
}