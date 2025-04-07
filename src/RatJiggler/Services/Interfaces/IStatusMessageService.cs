using System;

namespace RatJiggler.Services.Interfaces;

public interface IStatusMessageService
{
    event EventHandler<StatusMessageEventArgs> StatusMessageChanged;
    void SetStatusMessage(string message, string color = "Black");
}

public class StatusMessageEventArgs : EventArgs
{
    public string Message { get; }
    public string Color { get; }

    public StatusMessageEventArgs(string message, string color)
    {
        Message = message;
        Color = color;
    }
} 