namespace Share.Common.Helpers;
public static class Base64Helper
{
    /// <summary>
    /// Convert base64 string to byte array
    /// </summary>
    /// <param name="base64String">base64 string</param>
    /// <returns></returns>
    public static byte[] FromBase64String(this string base64String)
    {
        return Convert.FromBase64String(base64String);
    }
}
