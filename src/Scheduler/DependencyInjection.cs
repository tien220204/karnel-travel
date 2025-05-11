using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Settings;
using Scheduler.Interfaces;
using Scheduler.Service;
using Scheduler.Tasks;

namespace Schedule;
public static class DependencyInjection // Changed to static class to fix CS1106
{
	public static IServiceCollection AddSchedulerServices(this IServiceCollection services, IConfiguration configuration)
	{
		//using 
		var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();


		//services.AddSingleton<IElasticSearchService, ElasticSearchService>();

		services.AddHangfire(x => x.UseSqlServerStorage(hangfireSettings.ConnectionString, new SqlServerStorageOptions
		{
			CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
			SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
			QueuePollInterval = TimeSpan.Zero,
			UseRecommendedIsolationLevel = true,
			DisableGlobalLocks = true
		}));

		services.AddHangfireServer(options =>
		{
			options.WorkerCount = 1;
			options.Queues = new[] { "default" };
			options.ServerName = Environment.MachineName;
			options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
		});

		

		services.AddSingleton<ISchedulerTask, DemoTask>();
		services.AddSingleton<ISchedulerTask, ImportProcessingEventTask>();
		services.AddSingleton<ISchedulerTask, CleanDataHotelTask>();

		services.AddHostedService<RegisterScheduleService>();

		return services;
	}
}
