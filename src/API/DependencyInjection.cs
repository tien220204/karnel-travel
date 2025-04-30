using AutoMapper;
using FluentValidation;
using KarnelTravel.API.Services;
using KarnelTravel.Application.Common.Interfaces;
using KarnelTravel.Infrastructure.Data;
using KarnelTravel.Infrastructure.Models.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Security.Claims;
using System.Text.Json;

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

		// Thêm Keycloak Authentication
		var keycloakConfig = configuration.GetSection("Keycloak");

		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = keycloakConfig["Authority"];
				options.Audience = keycloakConfig["ClientId"];
				options.RequireHttpsMetadata = false;

				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					RoleClaimType = ClaimTypes.Role // defalut role
				};

				options.Events = new JwtBearerEvents
				{
					OnTokenValidated = ctx =>
					{
						var identity = ctx.Principal!.Identity as ClaimsIdentity;

						// resource access key in json body for realm
						var realmAccessClaim = ctx.Principal.FindFirst("realm_access");
						if (realmAccessClaim != null)
						{
							using var doc = JsonDocument.Parse(realmAccessClaim.Value);
							if (doc.RootElement.TryGetProperty("roles", out var roles))
							{
								foreach (var role in roles.EnumerateArray())
								{
									identity!.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
								}
							}
						}

						// resource access key in json body for client
						var resourceAccessClaim = ctx.Principal.FindFirst("resource_access");
						if (resourceAccessClaim != null)
						{
							using var doc = JsonDocument.Parse(resourceAccessClaim.Value);
							if (doc.RootElement.TryGetProperty(keycloakConfig["ClientId"]!, out var clientAccess))
							{
								if (clientAccess.TryGetProperty("roles", out var roles))
								{
									foreach (var role in roles.EnumerateArray())
									{
										identity!.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
									}
								}
							}
						}

						return Task.CompletedTask;
					}
				};
			});



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
