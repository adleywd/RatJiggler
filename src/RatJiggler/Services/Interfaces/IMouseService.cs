using System;
using RatJiggler.MouseUtilities.Windows;

namespace RatJiggler.Services.Interfaces;

public interface IMouseService
{
    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement);
    public void StartRealistic(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null);
    public void Stop();
}