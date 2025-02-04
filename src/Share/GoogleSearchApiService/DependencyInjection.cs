using KarnelTravel.Share.GoogleSearchApiService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarnelTravel.Share.GoogleSearchApiService;
public static class DependencyInjection
{
	public static IServiceCollection AddGoogleSearchApiService(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<GoogleSearchApiSettings>(configuration.GetSection(nameof(GoogleSearchApiSettings)));

		//services.AddScoped<IVngCloudStorageUploadService, VngCloudStorageUploadDataService>();

		return services;
	}
}
