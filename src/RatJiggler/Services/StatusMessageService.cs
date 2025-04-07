using System;
using RatJiggler.Services.Interfaces;

namespace RatJiggler.Services;

public class StatusMessageService : IStatusMessageService
{
    public event EventHandler<StatusMessageEventArgs>? StatusMessageChanged;

    public void SetStatusMessage(string message, string color = "Purple")
    {
        StatusMessageChanged?.Invoke(this, new StatusMessageEventArgs(message, color));
    }
} 