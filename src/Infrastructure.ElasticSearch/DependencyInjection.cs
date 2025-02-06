namespace Infrastructure.ElasticSearch;

using Infrastructure.ElasticSearch.Service;
using Infrastructure.ElasticSearch.Settings;
using KarnelTravel.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureElasticSearchServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<ElasticSettings>(configuration.GetSection(nameof(ElasticSettings)));

		services.AddSingleton<IElasticSearchService, ElasticSearchService>();

		return services;
	}
}
