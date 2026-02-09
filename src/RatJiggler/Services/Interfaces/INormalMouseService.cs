using System;

namespace RatJiggler.Services.Interfaces;

public interface INormalMouseService
{
    void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement, bool enableClick, int clickButton, int clickIntervalSeconds = 5, bool enableUserInterventionDetection = true, Action? onStopped = null);
    void Stop();
} 