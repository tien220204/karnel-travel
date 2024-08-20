using System.Text;
using System.Text.RegularExpressions;

namespace Share.Common.Extensions;

/// <summary>
/// Extension method for string
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Determines whether [is null or empty] [the specified value].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// If the value is NOT null or empty
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotNullNorEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Check if the value is null or whitespace only
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>
    ///   <c>true</c> if [is null or white space] [the specified value]; otherwise, <c>false</c>
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Check if the value is NOT null and whitespace only
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>
    ///   <c>true</c> if [is not null or white space] [the specified value]; otherwise, <c>false</c>
    /// </returns>
    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Trim and lower case the value
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string TransformLower(this string value)
    {
        return value?.Trim().ToLower();
    }


    /// <summary>
    /// Trim and upper case the value
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string TransformUpper(this string value)
    {
        return value?.Trim().ToUpper();
    }

    /// <summary>
    /// Trim the value safely
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string TrimSafe(this string value)
    {
        return value?.Trim();
    }

    /// <summary>
    /// Trim the suffix string if any
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="suffix">The suffix</param>
    /// <returns></returns>
    public static string TrimEnd(this string value, string suffix)
    {
        if (value.IsNullOrEmpty() || suffix.IsNullOrEmpty() ||
            !value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)) return value;

        return value.Substring(0, value.Length - suffix.Length);
    }

    /// <summary>
    /// Truncates the string if it exceeds the max length
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="maxLength">The maximum length</param>
    /// <returns></returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Escape the double quote (for using in javascript) by replacing " with ""
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns></returns>
    public static string EscapeJavascriptDoubleQuote(this string value)
    {
        return value.Replace("\"", "\\\"");
    }

    /// <summary>
    /// Shortcut to string.Format
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="args">The arguments</param>
    /// <returns></returns>
    public static string FormatString(this string value, params object[] args)
    {
        return string.Format(value, args);
    }

    /// <summary>
    /// Splits the and remove empty.
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="separator">separator</param>
    /// <returns></returns>
    public static string[] SplitAndRemoveEmpty(this string value, char separator)
    {
        return value.SplitAndRemoveEmpty(new char[] { separator });
    }

    /// <summary>
    /// Splits the and remove empty.
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="separators">separators</param>
    /// <returns></returns>
    public static string[] SplitAndRemoveEmpty(this string value, char[] separators)
    {
        return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Replace string with comparison option such as ignore case, etc
    /// </summary>
    /// <param name="source">source string</param>
    /// <param name="oldString">string to be replaced</param>
    /// <param name="newString">new string</param>
    /// <param name="comparison">comparison option</param>
    /// <returns></returns>
    public static string Replace(this string source, string oldString,
        string newString, StringComparison comparison)
    {
        var index = source.IndexOf(oldString, comparison);

        while (index > -1)
        {
            source = source.Remove(index, oldString.Length);
            source = source.Insert(index, newString);
            index = source.IndexOf(oldString, index + newString.Length, comparison);
        }

        return source;
    }

    /// <summary>
    /// Compare if two strings are equal (ignore case)
    /// </summary>
    /// <param name="firstString">first string</param>
    /// <param name="secondString">second string</param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string firstString, string secondString)
    {
        return firstString.Equals(secondString, StringComparison.InvariantCultureIgnoreCase);
    }

    public static byte[] ToByteArray(this string val)
    {
        return Encoding.UTF8.GetBytes(val);
    }

    /// <summary>
    /// Convert to Vietnamese unicode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToVietnameseUnicode(this string input)
    {
        input = Regex.Replace(input, @"à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
        input = Regex.Replace(input, @"À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ", "A");
        input = Regex.Replace(input, @"è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
        input = Regex.Replace(input, @"È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ", "E");
        input = Regex.Replace(input, @"ì|í|ị|ỉ|ĩ", "i");
        input = Regex.Replace(input, @"Ì|Í|Ị|Ỉ|Ĩ", "I");
        input = Regex.Replace(input, @"ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
        input = Regex.Replace(input, @"Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ", "O");
        input = Regex.Replace(input, @"ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
        input = Regex.Replace(input, @"Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ", "U");
        input = Regex.Replace(input, @"ỳ|ý|ỵ|ỷ|ỹ", "y");
        input = Regex.Replace(input, @"Ỳ|Ý|Ỵ|Ỷ|Ỹ", "Y");
        input = Regex.Replace(input, "đ", "d");
        input = Regex.Replace(input, "Đ", "D");

        input = Regex.Replace(input, @"\u0300|\u0301|\u0303|\u0309|\u0323", "");
        input = Regex.Replace(input, @"\u02C6|\u0306|\u031B", "");
        return input;
    }

    /// <summary>
    /// Convert to Vietnamese rergex pattern
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToVietnameseRegexPattern(this string input)
    {
        var vietnameseChars = new Dictionary<char, string>
        {
            {'a', "[aáàảãạâấầẩẫậăắằẳẵặ]"},
            {'e', "[eéèẻẽẹêếềểễệ]"},
            {'i', "[iíìỉĩị]"},
            {'o', "[oóòỏõọôốồổỗộơớờởỡợ]"},
            {'u', "[uúùủũụưứừửữự]"},
            {'y', "[yýỳỷỹỵ]"},
            {'d', "[dđ]"},
        };

        var result = string.Join("", input.Select(c => vietnameseChars.ContainsKey(c) ? vietnameseChars[c] : c.ToString()));
        return result;
    }
}
