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
}
