using System.Collections;
using UnityEngine;

/// <summary>
/// A collection of helper and utility functions.
/// </summary>
public static class Util
{
    /// <summary>
    /// Clamps the given value outside of the given start float and end float values.
    /// <br/>
    /// Returns the given value if it is outside the start and end range.
    /// </summary>
    /// <param name="value">The floating point value to restrict outside the range defined.</param>
    /// <param name="start">The lower bound of the range to compare against.</param>
    /// <param name="end">The upper bound of the range to compare against.</param>
    /// <returns></returns>
    public static float ClampOut(float value, float start, float end)
    {
        float dStart = start - value;
        float dEnd = value - end;

        if (dStart >= 0 || dEnd >= 0) return value;
        return dStart > dEnd ? start : end;
    }

    /// <summary>
    /// Invokes the provided function after the specified delay in seconds.
    /// </summary>
    /// <param name="delay">The number of seconds to delay the function by.</param>
    /// <param name="action">The funciton to run after the specified delay.</param>
    public static void RunAfter(float delay, System.Action action)
    {
        IEnumerator ThrowDelay()
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        God.instance.StartCoroutine(ThrowDelay());
    }

    /// <summary>
    /// Picks a random item from the provided array
    /// </summary>
    /// <typeparam name="T">The type of the array</typeparam>
    /// <param name="array">The array to get options from.</param>
    /// <returns>A random item from the array</returns>
    public static T PickRandom<T>(T[] array)
    {
        if (array.Length == 0) return default(T);
        return array[Random.Range(0, array.Length)];
    }

    /// <summary>
    /// Converts a position in the world to a position on the canvas.
    /// </summary>
    /// <param name="canvas">The canvas to get a position on</param>
    /// <param name="position">The world position</param>
    /// <returns>The canvas position</returns>
    public static Vector2 WorldToCanvasPosition(Canvas canvas, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

        var rootCanvasTransform = (canvas.isRootCanvas ? canvas.transform : canvas.rootCanvas.transform) as RectTransform;
        var rootCanvasSize = rootCanvasTransform!.rect.size;
        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        var rootCoord = (viewportPoint - rootCanvasTransform.pivot) * rootCanvasSize;
        if (canvas.isRootCanvas)
            return rootCoord;

        var rootToWorldPos = rootCanvasTransform.TransformPoint(rootCoord);
        return canvas.transform.InverseTransformPoint(rootToWorldPos);
    }
}