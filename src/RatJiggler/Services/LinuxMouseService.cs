using System;
using RatJiggler.MouseUtilities.Windows;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class LinuxMouseService : INormalMouseService, IRealisticMouseService
{
    public void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement)
    {
        throw new NotImplementedException();
    }

    public void StartRealistic(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}