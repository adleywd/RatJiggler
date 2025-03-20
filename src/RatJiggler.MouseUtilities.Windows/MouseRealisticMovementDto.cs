using System.Drawing;

namespace RatJiggler.MouseUtilities.Windows;

public record MouseRealisticMovementDto(
    Rectangle ScreenBounds,
    int MinSpeed,
    int MaxSpeed,
    bool EnableStepPauses,
    int StepPauseMin,
    int StepPauseMax,
    bool EnableRandomPauses,
    int RandomPauseProbability,
    int RandomPauseMin,
    int RandomPauseMax,
    int? RandomSeed,
    float HorizontalBias,
    float VerticalBias,
    float PaddingPercentage,
    bool EnableUserInterventionDetection,
    int MovementThresholdInPixels);