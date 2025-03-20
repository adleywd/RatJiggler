using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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
    /// <param name="screenBounds">The bounds of the screen.</param>
    /// <param name="minSpeed">The minimum speed of the mouse movement (1 = slowest, 10 = fastest).</param>
    /// <param name="maxSpeed">The maximum speed of the mouse movement (1 = slowest, 10 = fastest).</param>
    /// <param name="enableStepPauses">Whether to include small pauses between each step.</param>
    /// <param name="stepPauseMin">Minimum delay between steps (in milliseconds).</param>
    /// <param name="stepPauseMax">Maximum delay between steps (in milliseconds).</param>
    /// <param name="enableRandomPauses">Whether to include longer random pauses after some time.</param>
    /// <param name="randomPauseProbability">Probability of a random pause (0 to 100).</param>
    /// <param name="randomPauseMin">Minimum duration of a random pause (in milliseconds).</param>
    /// <param name="randomPauseMax">Maximum duration of a random pause (in milliseconds).</param>
    /// <param name="horizontalBias">Bias towards horizontal movement (-1 to 1). Positive values favor right, negative values favor left.</param>
    /// <param name="verticalBias">Bias towards vertical movement (-1 to 1). Positive values favor down, negative values favor up.</param>
    /// <param name="paddingPercentage">Percentage of screen bounds to use as padding (0 to 0.5).</param>
    /// <param name="randomSeed">Optional random seed for reproducible behavior.</param>
    /// <param name="cancellationToken">The cancellation token to stop the movement.</param>
    public static void MoveRealistic(
        MouseRealisticMovementDto dto,
        CancellationToken cancellationToken = default)
    {
        // Validate inputs
        var minSpeed = Math.Clamp(dto.MinSpeed, 1, 10);
        var maxSpeed = Math.Clamp(dto.MaxSpeed, 1, 10);
        if (minSpeed > maxSpeed)
        {
            minSpeed = maxSpeed;
        }

        var randomPauseProbability = Math.Clamp(dto.RandomPauseProbability, 0, 100);
        var paddingPercentage = Math.Clamp(dto.PaddingPercentage, 0, 0.5f);
        var horizontalBias = Math.Clamp(dto.HorizontalBias, -1, 1);
        var verticalBias = Math.Clamp(dto.VerticalBias, -1, 1);

        // Initialize random number generator
        Random random = dto.RandomSeed.HasValue ? new Random(dto.RandomSeed.Value) : new Random();

        // Define realistic padding
        var paddingX = (int)(dto.ScreenBounds.Width * paddingPercentage);
        var paddingY = (int)(dto.ScreenBounds.Height * paddingPercentage);

        var startX = dto.ScreenBounds.Left + paddingX;
        var startY = dto.ScreenBounds.Top + paddingY;
        var endX = dto.ScreenBounds.Right - paddingX;
        var endY = dto.ScreenBounds.Bottom - paddingY;

        // Get the initial mouse position
        Point currentPosition = GetMousePosition();
        var currentX = currentPosition.X;
        var currentY = currentPosition.Y;

        // Initialize direction variables
        var directionX = random.Next(-5, 6); // Random initial X direction (-5 to 5 pixels)
        var directionY = random.Next(-5, 6); // Random initial Y direction (-5 to 5 pixels)

        while (!cancellationToken.IsCancellationRequested)
        {
            // Track the current mouse position
            currentPosition = GetMousePosition();
            currentX = currentPosition.X;
            currentY = currentPosition.Y;

            // Adjust direction if close to the padding
            if (currentX + directionX < startX || currentX + directionX > endX)
            {
                directionX = -directionX; // Reverse X direction
            }

            if (currentY + directionY < startY || currentY + directionY > endY)
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
            if (dto.EnableStepPauses)
            {
                int randomDelay = random.Next(dto.StepPauseMin, dto.StepPauseMax + 1);
                Thread.Sleep(randomDelay);
            }

            // Randomly introduce a longer pause to mimic human behavior if enabled
            if (dto.EnableRandomPauses && random.Next(0, 100) < randomPauseProbability)
            {
                int pauseDuration = random.Next(dto.RandomPauseMin, dto.RandomPauseMax + 1);
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
        INPUT[] inputArray = { input };
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