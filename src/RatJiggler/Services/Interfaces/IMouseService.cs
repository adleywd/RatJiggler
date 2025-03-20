using System;

namespace RatJiggler.Services.Interfaces;

public interface IMouseService
{
    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement);
    public void StartRealistic(Action? onStopped = null);
    public void Stop();
}