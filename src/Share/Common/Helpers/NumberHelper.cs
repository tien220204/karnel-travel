using System.Globalization;

namespace Share.Common.Helpers;
public static class NumberHelper
{
    static NumberHelper()
    {
    }

    /// <summary>
    /// Gets the value in range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val">value</param>
    /// <param name="minValue">minimum value</param>
    /// <param name="maxValue">maximum value</param>
    /// <returns></returns>
    public static T GetValueInRange<T>(T val, T? minValue = null, T? maxValue = null)
        where T : struct, IComparable
    {
        var returnVal = val;
        if (minValue.HasValue)
        {
            if (returnVal.CompareTo(minValue.Value) < 0)
            {
                returnVal = minValue.Value;
            }
        }
        if (maxValue.HasValue)
        {
            if (returnVal.CompareTo(maxValue.Value) > 0)
            {
                returnVal = maxValue.Value;
            }
        }
        return returnVal;
    }

    /// <summary>
    /// Determines whether [is value in range] [the specified value].
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val">value</param>
    /// <param name="minValue">minimum value</param>
    /// <param name="maxValue">maximum value</param>
    /// <returns>
    ///   <c>true</c> if [is value in range] [the specified value]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValueInRange<T>(T val, T? minValue = null, T? maxValue = null)
        where T : struct, IComparable
    {
        var isInRange = true;
        if (minValue.HasValue)
        {
            if (val.CompareTo(minValue.Value) < 0)
            {
                isInRange = false;
            }
        }
        if (maxValue.HasValue)
        {
            if (val.CompareTo(maxValue.Value) > 0)
            {
                isInRange = false;
            }
        }
        return isInRange;
    }

    /// <summary>
    /// Determines whether [has non negative value] [the specified value].
    /// </summary>
    /// <param name="val">value</param>
    /// <param name="minValue">minimum value</param>
    /// <returns>
    ///   <c>true</c> if [has non negative value] [the specified value]; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasNonNegativeValue(int? val, int minValue = 0)
    {
        return val.HasValue && val.Value >= 0 && val.Value > minValue;
    }

    /// <summary>
    /// Gets the int value.
    /// </summary>
    /// <param name="val">value</param>
    /// <param name="defaultVal">default value</param>
    /// <returns></returns>
    public static int GetIntVal(this int? val, int defaultVal = 0)
    {
        return val ?? defaultVal;
    }

    /// <summary>
    /// Determines the minimum of the parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">values</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The passed array cannot be null</exception>
    public static T Min<T>(this T[] values) where T : IComparable
    {
        if (values == null)
        {
            throw new ArgumentException("The passed array cannot be null");
        }
        var minVal = values[0];
        for (int ind = 0; ind < values.Length; ind++)
        {
            if (minVal.CompareTo(values[ind]) > 0)
            {
                minVal = values[ind];
            }
        }
        return minVal;
    }

    /// <summary>
    /// Determines the maximun of the parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">values</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The passed array cannot be null</exception>
    public static T Max<T>(this T[] values) where T : IComparable
    {
        if (values == null)
        {
            throw new ArgumentException("The passed array cannot be null");
        }
        var maxVal = values[0];
        for (int ind = 0; ind < values.Length; ind++)
        {
            if (maxVal.CompareTo(values[ind]) < 0)
            {
                maxVal = values[ind];
            }
        }
        return maxVal;
    }

    /// <summary>
    /// TryParse function delegate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">value</param>
    /// <param name="result">result</param>
    /// <returns></returns>
    public delegate bool TryParseHandler<T>(string value, out T result);

    /// <summary>
    /// Tries to parse number
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">value</param>
    /// <param name="handler">handler</param>
    /// <returns></returns>
    public static T? TryParse<T>(string value, TryParseHandler<T> handler) where T : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        else
        {
            if (handler(value, out T result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Determines whether [is range valid] [the specified range].
    /// </summary>
    /// <param name="range">range</param>
    /// <param name="minValue">minimum value</param>
    /// <param name="maxValue">maximum value</param>
    /// <returns>
    ///   <c>true</c> if [is range valid] [the specified range]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRangeValid(int?[] range, int? minValue = null, int? maxValue = null)
    {
        var isRangeValid = range != null && range.Length == 2
                       && range[0].HasValue && range[1].HasValue
                       && range[1].Value >= range[0].Value;
        if (minValue.HasValue)
        {
            isRangeValid &= range[0].Value >= minValue.Value;
        }
        if (maxValue.HasValue)
        {
            isRangeValid &= range[1].Value <= maxValue.Value;
        }
        return isRangeValid;
    }

    /// <summary>
    /// Calculates the maximum value.
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="maximumRate">maximum rate</param>
    /// <param name="maximumReduction">maximum reduction</param>
    /// <returns></returns>
    public static double CalculateMaximumValue(double value, double maximumRate, double maximumReduction)
    {
        return Math.Min(value * maximumRate, value - maximumReduction);
    }

    /// <summary>
    /// Convert radian to degree.
    /// </summary>
    /// <param name="rad">radian value</param>
    /// <returns></returns>
    public static double RadToDegree(double rad)
    {
        return rad * 180 / Math.PI;
    }

    /// <summary>
    /// Convert degree to radian
    /// </summary>
    /// <param name="degree">degree value</param>
    /// <returns></returns>
    public static double DegreeToRad(double degree)
    {
        return degree * Math.PI / 180;
    }

    /// <summary>
    /// Tries parse decimal number
    /// </summary>
    /// <param name="number">number</param>
    /// <param name="value">value</param>
    /// <returns></returns>
    public static bool TryParseDecimal(string number, out decimal value)
    {
        return decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
    }
}
