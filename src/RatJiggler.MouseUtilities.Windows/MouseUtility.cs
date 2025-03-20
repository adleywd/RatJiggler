using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace RatJiggler.MouseUtilities.Windows;

[SupportedOSPlatform("windows5.0")]
public static class MouseUtility
{
    /// <summary>
    /// Moves the mouse by the specified delta.
    /// </summary>
    /// <param name="moveX">The number of pixels to move along the X axis.</param>
    /// <param name="moveY">The number of pixels to move along the Y axis.</param>
    public static void Move(int moveX, int moveY)
    {
        var input = CreateMouseInput(moveX, moveY, MOUSE_EVENT_FLAGS.MOUSEEVENTF_MOVE);
        SendInput(input);
    }

    /// <summary>
    /// Moves the mouse realistically within the specified screen bounds.
    /// </summary>
    /// <param name="mouseMovementDto">Mouse realistic movement DTO</param>
    /// <param name="onStopped">On stop callback</param>
    /// <param name="cancellationToken">Cancellation token used to stop the movement</param>
    public static void MoveRealistic(
    MouseRealisticMovementDto mouseMovementDto,
    Action? onStopped = null,
    CancellationToken cancellationToken = default)
{
    // Validate inputs
    var minSpeed = Math.Clamp(mouseMovementDto.MinSpeed, 1, 10);
    var maxSpeed = Math.Clamp(mouseMovementDto.MaxSpeed, 1, 10);
    if (minSpeed > maxSpeed)
    {
        minSpeed = maxSpeed;
    }

    var randomPauseProbability = Math.Clamp(mouseMovementDto.RandomPauseProbability, 0, 100);
    var paddingPercentage = Math.Clamp(mouseMovementDto.PaddingPercentage, 0, 0.5f);
    var horizontalBias = Math.Clamp(mouseMovementDto.HorizontalBias, -1, 1);
    var verticalBias = Math.Clamp(mouseMovementDto.VerticalBias, -1, 1);

    // Initialize random number generator
    Random random = mouseMovementDto.RandomSeed.HasValue ? new Random(mouseMovementDto.RandomSeed.Value) : new Random();

    // Define realistic padding
    var paddingX = (int)(mouseMovementDto.ScreenBounds.Width * paddingPercentage);
    var paddingY = (int)(mouseMovementDto.ScreenBounds.Height * paddingPercentage);

    var startX = mouseMovementDto.ScreenBounds.Left + paddingX;
    var startY = mouseMovementDto.ScreenBounds.Top + paddingY;
    var endX = mouseMovementDto.ScreenBounds.Right - paddingX;
    var endY = mouseMovementDto.ScreenBounds.Bottom - paddingY;

    // Get the initial mouse position
    Point initialPosition = GetMousePosition();
    Point lastPosition = initialPosition;

    // Initialize direction variables
    var directionX = random.Next(-5, 6); // Random initial X direction (-5 to 5 pixels)
    var directionY = random.Next(-5, 6); // Random initial Y direction (-5 to 5 pixels)

    while (!cancellationToken.IsCancellationRequested)
    {
        // Get the current mouse position
        Point currentPosition = GetMousePosition();

        // Check if the mouse has moved (user intervention) if enabled
        if (mouseMovementDto.EnableUserInterventionDetection)
        {
            // Calculate the distance moved since the last position
            var deltaX = currentPosition.X - lastPosition.X;
            var deltaY = currentPosition.Y - lastPosition.Y;
            var distanceMoved = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            // If the distance moved exceeds the threshold, stop the realistic movement
            if (distanceMoved > mouseMovementDto.MovementThresholdInPixels)
            {
                onStopped?.Invoke();
                Console.WriteLine("Significant mouse movement detected. Stopping realistic movement...");
                // break; // Exit the loop if the user moves the mouse significantly
            }
        }

        // Track the current mouse position
        lastPosition = currentPosition;

        // Adjust direction if close to the padding
        if (currentPosition.X + directionX < startX || currentPosition.X + directionX > endX)
        {
            directionX = -directionX; // Reverse X direction
        }

        if (currentPosition.Y + directionY < startY || currentPosition.Y + directionY > endY)
        {
            directionY = -directionY; // Reverse Y direction
        }

        // Add slight randomness to the direction
        directionX += random.Next(-1, 2); // Small random change in X direction
        directionY += random.Next(-1, 2); // Small random change in Y direction

        // Apply directional bias
        directionX = (int)(directionX * (1 + horizontalBias));
        directionY = (int)(directionY * (1 + verticalBias));

        // Adjust step size based on speed
        int stepSize = random.Next(minSpeed, maxSpeed + 1); // Randomize speed within the range
        directionX = Math.Clamp(directionX, -stepSize, stepSize);
        directionY = Math.Clamp(directionY, -stepSize, stepSize);

        // Move the mouse incrementally
        Move(directionX, directionY);

        // Add a small delay between steps if enabled
        if (mouseMovementDto.EnableStepPauses)
        {
            int randomDelay = random.Next(mouseMovementDto.StepPauseMin, mouseMovementDto.StepPauseMax + 1);
            Thread.Sleep(randomDelay);
        }

        // Randomly introduce a longer pause to mimic human behavior if enabled
        if (mouseMovementDto.EnableRandomPauses && random.Next(0, 100) < randomPauseProbability)
        {
            int pauseDuration = random.Next(mouseMovementDto.RandomPauseMin, mouseMovementDto.RandomPauseMax + 1);
            Thread.Sleep(pauseDuration);
        }
    }
}

    /// <summary>
    /// Gets the current mouse position.
    /// </summary>
    /// <returns>The current mouse position as a <see cref="Point"/>.</returns>
    private static Point GetMousePosition()
    {
        PInvoke.GetCursorPos(out Point point);
        return new Point(point.X, point.Y);
    }

    /// <summary>
    /// Creates a mouse input structure for the specified movement.
    /// </summary>
    /// <param name="dx">The number of pixels to move along the X axis.</param>
    /// <param name="dy">The number of pixels to move along the Y axis.</param>
    /// <param name="flags">The mouse event flags.</param>
    /// <returns>An <see cref="INPUT"/> structure representing the mouse movement.</returns>
    private static INPUT CreateMouseInput(int dx, int dy, MOUSE_EVENT_FLAGS flags)
    {
        return new INPUT
        {
            type = INPUT_TYPE.INPUT_MOUSE,
            Anonymous = new INPUT._Anonymous_e__Union
            {
                mi = new MOUSEINPUT
                {
                    dx = dx,
                    dy = dy,
                    mouseData = 0,
                    dwFlags = flags,
                    time = 0,
                    dwExtraInfo = (UIntPtr)IntPtr.Zero
                }
            }
        };
    }

    /// <summary>
    /// Sends the specified input to the system.
    /// </summary>
    /// <param name="input">The input to send.</param>
    private static void SendInput(INPUT input)
    {
        INPUT[] inputArray =
        {
            input
        };
        var inputSpan = new Span<INPUT>(inputArray);

#pragma warning disable CA1416
        var inputResult = PInvoke.SendInput(inputSpan, Marshal.SizeOf<INPUT>());
#pragma warning restore CA1416

        if (inputResult != 1)
        {
            var errorCode = Marshal.GetLastWin32Error();
            throw new InvalidOperationException(
                $"Failed to insert event into input stream; result={inputResult}, errorCode=0x{errorCode:x8}");
        }
    }
}