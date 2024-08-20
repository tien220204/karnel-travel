namespace KarnelTravel.Share.Cache.Settings;
public class FushionCacheSettings
{
    public bool EnableDistributedCache { get; set; }
    public string RedisConnectionString { get; set; }
    public int CacheDurationInMinutes { get; set; } = 60;
    public int MaxSafeDurationInMinutes { get; set; } = 120;
}
