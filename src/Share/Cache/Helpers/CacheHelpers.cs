namespace KarnelTravel.Share.Cache.Helpers;
public static class CacheHelpers
{
    public static string BuildCacheKey(string prefix, string detail) => prefix + "_" + detail;
}
