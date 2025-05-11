using Hangfire.Common;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scheduler.Interfaces;

namespace Scheduler.Service;
public class RegisterScheduleService : IHostedService
{
	private readonly ILogger<RegisterScheduleService> _logger;
	private readonly IRecurringJobManager _recurringJobManager;
	private readonly IEnumerable<ISchedulerTask> _services;
	public RegisterScheduleService(
		ILogger<RegisterScheduleService> logger,
		IRecurringJobManager recurringJobManager,
		IEnumerable<ISchedulerTask> services)
	{
		_logger = logger;
		_recurringJobManager = recurringJobManager;
		_services = services;
	}
	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Register Schedule Service is starting.");
		foreach (var service in _services)
		{
			_recurringJobManager.AddOrUpdate(
				service.Name,
				Job.FromExpression(() => service.ExecuteJob()),
				service.CronTime,
				new RecurringJobOptions
				{
					QueueName = service.GetQueueName(),
					TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
				});
		}
		return Task.CompletedTask;
	}
	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Register Schedule Service is stopping.");
		return Task.CompletedTask;
	}

}
