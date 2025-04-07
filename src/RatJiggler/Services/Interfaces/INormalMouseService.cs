namespace RatJiggler.Services.Interfaces;

public interface INormalMouseService
{
    void Start(int moveX, int moveY, int secondsBetweenMovement, bool backAndForthMovement);
    void Stop();
} 