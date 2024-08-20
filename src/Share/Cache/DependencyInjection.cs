using KarnelTravel.Share.Cache.Settings;
using KarnelTravel.Share.CloudinaryService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;

namespace KarnelTravel.Share.Cache;
public static class DependencyInjection
{
    public static void AddApplicationCache(this IServiceCollection services, IConfiguration configuration, Action<FusionCacheOptions> setupOptionsAction = null)
    {
        services.AddMemoryCache();

        // Bind settings
        var cacheSettings = configuration.GetSection(nameof(CloudinaryOAuthApiSettings)).Get<FushionCacheSettings>();

        if (cacheSettings.EnableDistributedCache)
        {
            if (string.IsNullOrEmpty(cacheSettings.RedisConnectionString))
            {
                throw new ArgumentNullException(nameof(FushionCacheSettings.RedisConnectionString));
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheSettings.RedisConnectionString;
            });

            services.AddFusionCacheNewtonsoftJsonSerializer();
            services.AddFusionCacheStackExchangeRedisBackplane(options =>
            {
                options.Configuration = cacheSettings.RedisConnectionString;
            });
        }

        //https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/DependencyInjection.md
        if (setupOptionsAction != null)
        {
            services.AddFusionCache().WithOptions(setupOptionsAction);
        }
        else
        {
            services.AddFusionCache().WithOptions(options => options.DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2))
                .WithDefaultEntryOptions(options =>
                {
                    options.Duration = TimeSpan.FromMinutes(cacheSettings.CacheDurationInMinutes);
                    options.IsFailSafeEnabled = true;
                    options.FailSafeMaxDuration = TimeSpan.FromMinutes(cacheSettings.MaxSafeDurationInMinutes);
                    options.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);
                    options.FactorySoftTimeout = TimeSpan.FromMilliseconds(100);
                    options.FactoryHardTimeout = TimeSpan.FromMilliseconds(5000);
                    options.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                    options.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
                    options.AllowBackgroundDistributedCacheOperations = true;
                    options.JitterMaxDuration = TimeSpan.FromSeconds(2);
                });
        }
    }
}
