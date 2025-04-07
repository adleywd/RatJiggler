using System;
using System.Threading.Tasks;
using RatJiggler.MouseUtilities.Windows;

namespace RatJiggler.Services.Interfaces;

public interface IRealisticMouseService
{
    void StartRealistic(MouseRealisticMovementDto mouseRealisticMovementDto, Action? onStopped = null);
    void Stop();
} 