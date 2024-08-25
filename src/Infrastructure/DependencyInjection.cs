using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Infrastructure.Data;
using KarnelTravel.Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KarnelTravel.Infrastructure;
public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");

		Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

		services.AddDbContext<ApplicationDbContext>((sp, options) =>
		{
			options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

			options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
		});

		services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddAuthentication()
			.AddBearerToken(IdentityConstants.BearerScheme);

		services.AddAuthorizationBuilder();

		services.AddSingleton(TimeProvider.System);
		services.AddTransient<IIdentityService, IdentityService>();

		services.AddAuthorization(options =>
			options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

		return services;
	}
}