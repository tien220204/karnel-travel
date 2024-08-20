namespace Share.Common.Extensions;
/// <summary>
/// The guid extension.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// The new.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string New()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// The parse safe.
    /// </summary>
    /// <param name="val">
    /// The guid string.
    /// </param>
    /// <returns>
    /// The <see cref="Guid"/>.
    /// </returns>
    public static Guid? ToGuid(this string val)
    {
        if (Guid.TryParse(val, out var guid))
        {
            return guid;
        }
        return null;
    }

    /// <summary>
    /// The is null or empty.
    /// </summary>
    /// <param name="guid">
    /// The guid.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return !guid.HasValue || guid == Guid.Empty;
    }

    /// <summary>
    /// The is not null or empty.
    /// </summary>
    /// <param name="guid">
    /// The guid.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsNotNullNorEmpty(this Guid? guid)
    {
        return guid.HasValue && guid != Guid.Empty;
    }

    /// <summary>
    /// The is empty.
    /// </summary>
    /// <param name="guid">
    /// The guid.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsEmpty(this Guid guid)
    {
        return guid == Guid.Empty;
    }

    /// <summary>
    /// The is not empty.
    /// </summary>
    /// <param name="guid">
    /// The guid.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public static bool IsNotEmpty(this Guid guid)
    {
        return guid != Guid.Empty;
    }

    /// <summary>
    /// The list string to guids.
    /// </summary>
    /// <param name="stringGuids">
    /// The string guids.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerable"/>.
    /// </returns>
    public static IEnumerable<Guid?> ListStringToGuids(this List<string> stringGuids)
    {
        foreach (var stringGuid in stringGuids)
        {
            yield return stringGuid.ToGuid();
        }
    }
}
