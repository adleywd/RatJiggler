using System;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class LinuxMouseService : IMouseService
{
    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement)
    {
        throw new NotImplementedException();
    }

    public void StartRealistic(Action? onStopped = null)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}