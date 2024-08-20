using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Share.Cache.Extensions;
public static class CacheExtensions
{
    /// <summary>
    /// Set cache for quickly response - use for kafka message - only save for 15 seconds
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="fusionCache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task SetForQuicklyResponseAsync<TValue>(this IFusionCache fusionCache, string key, TValue value, CancellationToken token = default)
    {
        await fusionCache.SetAsync(key, value, options => options.SetFailSafe(false).SetDurationSec(15), token);
    }

    /// <summary>
    /// Set cache for long term using
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="fusionCache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="longTermMinutes">Default is 1440 minutes</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task SetForLongTermAsync<TValue>(this IFusionCache fusionCache, string key, TValue value, int longTermMinutes = 1440, CancellationToken token = default)
    {
        await fusionCache.SetAsync(key, value, options => options.SetDurationMin(longTermMinutes), token);
    }
}
