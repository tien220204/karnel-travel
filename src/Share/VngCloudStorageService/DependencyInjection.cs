using KarnelTravel.Share.VngCloudStorageService.Interfaces;
using KarnelTravel.Share.VngCloudStorageService.Services;
using KarnelTravel.Share.VngCloudStorageService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarnelTravel.Share.VngCloudStorageService;
public static class DependencyInjection
{
	public static IServiceCollection AddVngCloudStorageServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<VngCloudStorageOAuthApiSettings>(configuration.GetSection(nameof(VngCloudStorageOAuthApiSettings)));

		services.AddScoped<IVngCloudStorageUploadService, VngCloudStorageUploadDataService>();

		return services;
	}
}
