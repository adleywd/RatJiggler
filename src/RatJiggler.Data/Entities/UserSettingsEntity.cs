using System.ComponentModel.DataAnnotations;

namespace RatJiggler.Data.Entities;

public class UserSettingsEntity
{
    [Key]
    public int Id { get; init; }
    public int MoveX { get; set; }
    public int MoveY { get; set; }
    public int Duration { get; set; }
    public bool BackForth { get; set; }
    public int MinSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public bool EnableStepPauses { get; set; }
    public int StepPauseMin { get; set; }
    public int StepPauseMax { get; set; }
    public bool EnableRandomPauses { get; set; }
    public int RandomPauseProbability { get; set; }
    public int RandomPauseMin { get; set; }
    public int RandomPauseMax { get; set; }
    public float HorizontalBias { get; set; }
    public float VerticalBias { get; set; }
    public float PaddingPercentage { get; set; }
    public int SelectedMouseMovementModeIndex { get; set; }
    public int? RandomSeed { get; set; }
    public bool EnableUserInterventionDetection { get; set; }
    public int MovementThresholdInPixels { get; set; }
} 