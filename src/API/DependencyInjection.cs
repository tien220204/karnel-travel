using AutoMapper;
using Duende.IdentityServer.Validation;
using FluentValidation;
using KarnelTravel.API.Custom;
using KarnelTravel.API.Services;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Domain.Entities.Features.Users;
using KarnelTravel.Infrastructure.Data;
using KarnelTravel.Infrastructure.Models.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;

namespace KarnelTravel.API;

public static class DependencyInjection
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
	{
		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddScoped<IUser, CurrentUser>();

		services.AddHttpContextAccessor();
		
		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();

		services.AddExceptionHandler<CustomExceptionHandler>();

		services.AddRazorPages();

		services.AddCors();

		// Customise default API behaviour
		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true);

		services.AddEndpointsApiExplorer();

		services.AddOpenApiDocument((configure, sp) =>
		{
			configure.Title = "KarnelTravel API";

			// Add JWT
			configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
			{
				Type = OpenApiSecuritySchemeType.ApiKey,
				Name = "Authorization",
				In = OpenApiSecurityApiKeyLocation.Header,
				Description = "Type into the textbox: Bearer {your JWT token}."
			});

			configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
		});

		services.AddApiVersioning(config =>
		{
			config.DefaultApiVersion = new ApiVersion(1, 0);
			config.AssumeDefaultVersionWhenUnspecified = true;
			config.ReportApiVersions = true;
		});

		services.AddVersionedApiExplorer(options =>
		{
			// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			// note: the specified format code will format the version as "'v'major[.minor][-status]"
			options.GroupNameFormat = "'v'VVV";

			// note: this option is only necessary when versioning by url segment. the SubstitutionFormat
			// can also be used to control the format of the API version in route templates
			options.SubstituteApiVersionInUrl = true;
		});

		services.AddRouting(options => options.LowercaseUrls = true);
		ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

		services.AddHttpClient();

		services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
		{
			options.SignIn.RequireConfirmedAccount = false;
			options.SignIn.RequireConfirmedEmail = false;
			options.SignIn.RequireConfirmedPhoneNumber = false;

			options.Password = new PasswordOptions
			{
				RequiredLength = 1,
				RequiredUniqueChars = 0,
				RequireNonAlphanumeric = false,
				RequireLowercase = false,
				RequireUppercase = false,
				RequireDigit = false,
			};

			options.Lockout = new LockoutOptions
			{
				AllowedForNewUsers = true,
				MaxFailedAccessAttempts = 999,
			};
			options.User.RequireUniqueEmail = true;
		})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders()
			.AddApiEndpoints();

		var builder = services.AddIdentityServer(options =>
		{
			options.Events.RaiseErrorEvents = true;
			options.Events.RaiseInformationEvents = true;
			options.Events.RaiseFailureEvents = true;
			options.Events.RaiseSuccessEvents = true;

			// see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
			options.EmitStaticAudienceClaim = true;

			//if (webHostEnvironment.IsProduction())
			//{
			//    options.LicenseKey = LoadLicenseKey(webHostEnvironment);
			//}
		}).AddInMemoryIdentityResources(IdentitySeverConfigs.IdentityResources)
		  .AddInMemoryApiScopes(IdentitySeverConfigs.ApiScopes)
		  .AddInMemoryClients(IdentitySeverConfigs.GetClients(configuration))
		  .AddInMemoryApiResources(IdentitySeverConfigs.GetApiResources(configuration))
		  .AddAspNetIdentity<ApplicationUser>()
		  .AddProfileService<CustomProfileService>();

		var connectStr = configuration.GetConnectionString("DefaultConnection");
		var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;

		builder.AddOperationalStore(options =>
		{
			options.ConfigureDbContext = b => b.UseNpgsql(connectStr,
				sql => sql.MigrationsAssembly(migrationsAssembly));

			// this enables automatic token cleanup. this is optional.
			options.EnableTokenCleanup = true;
			options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
		});

		services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
		services.AddScoped<UserManager<ApplicationUser>, CustomUserManager<ApplicationUser>>();

		return services;
	}

	public static IServiceCollection ConfigMapper(this IServiceCollection services)
	{
		var config = new MapperConfiguration(cfg => { ModelMapper.MappingDto(cfg); });
		var mapper = config.CreateMapper();

		services.AddSingleton(mapper);
		return services;
	}
}
