namespace KarnelTravel.Share.Common.Helpers;
public static class AutoMapperHelper
{
    public static string GuidToStringConverter(Guid guid)
    {
        return guid.ToString();
    }

    public static string NullableGuidToStringConverter(Guid? guid)
    {
        return guid.HasValue ? guid.ToString() : null;
    }
}
