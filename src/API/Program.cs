using KarnelTravel.API;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;
using KarnelTravel.Share.Localization;
using KarnelTravel.API.Logging;
using KarnelTravel.Infrastructure.Data;
using KarnelTravel.Application;
using HealthChecks.UI.Client;
using KarnelTravel.Infrastructure;
using KarnelTravel.Share.CloudinaryService;
using KarnelTravel.Share.Cache;
using Infrastructure.ElasticSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureElasticSearchServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCloudinaryServices(builder.Configuration);
builder.Services.AddApplicationCache(builder.Configuration);
builder.Services.ConfigMapper();

builder.Services.AddLocalizationSupport("vi");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("vi") };
	options.DefaultRequestCulture = new RequestCulture("vi");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
	loggerFactory?.AddFileSerilog(builder.Configuration, builder.Environment.ContentRootPath);

	
}
else
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();

	//app.Use(async (ctx, next) =>
	//{
	//    var serverUrls = ctx.RequestServices.GetRequiredService<IServerUrls>();
	//    serverUrls.Origin = builder.Configuration?.GetValue<string>("Jwt:Authority") ?? throw new Exception($"Authority can not be null"); ;
	//    await next();
	//});

	await app.InitialiseDatabaseAsync();
}

await ApplicationDbContextSeed.SeedProductUnitAsync(app.Services);

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseLocalizationSupport();

app.UseSwaggerUi(settings =>
{
	settings.Path = "/api";
	settings.DocumentPath = "/api/specification.json";
});

app.UseCors(config => config
	.AllowAnyOrigin()
	.AllowAnyHeader()
	.AllowAnyMethod());

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
	Predicate = _ => true,
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/liveness", new HealthCheckOptions
{
	Predicate = r => r.Name.Contains("self")
});

app.Run();
