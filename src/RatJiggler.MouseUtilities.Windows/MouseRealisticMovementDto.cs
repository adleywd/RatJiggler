using System.Drawing;

namespace RatJiggler.MouseUtilities.Windows;

public record MouseRealisticMovementDto(
    Rectangle ScreenBounds,
    string StatusMessage,
    int MoveX,
    int MoveY,
    int Duration,
    bool BackForth,
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
    double HorizontalBias,
    double VerticalBias,
    double PaddingPercentage);