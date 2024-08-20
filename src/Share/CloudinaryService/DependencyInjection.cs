using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KarnelTravel.Share.CloudinaryService.Interfaces;
using KarnelTravel.Share.CloudinaryService.Services;
using KarnelTravel.Share.CloudinaryService.Settings;

namespace KarnelTravel.Share.CloudinaryService;
public static class DependencyInjection
{
    public static IServiceCollection AddCloudinaryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinaryOAuthApiSettings>(configuration.GetSection(nameof(CloudinaryOAuthApiSettings)));

        services.AddScoped<ICloudinaryUploadService, CloudinaryStorageDataService>();

        return services;
    }
}
