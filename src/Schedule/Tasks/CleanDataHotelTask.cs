using KarnelTravel.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Scheduler.Interfaces;

namespace Scheduler.Tasks;


public class CleanDataHotelTask : ISchedulerTask
{
	private readonly ILogger<CleanDataHotelTask> _logger;
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IElasticSearchService _elasticSearchService;

	public CleanDataHotelTask(
		ILogger<CleanDataHotelTask> logger,
		IServiceScopeFactory scopeFactory,
		IElasticSearchService elasticSearchService)
	{
		_logger = logger;
		_scopeFactory = scopeFactory;
		_elasticSearchService = elasticSearchService;
	}

	public string Name => nameof(CleanDataHotelTask);

	/// <summary>
	/// Run every 1 minute
	/// </summary>
	public string CronTime => "0 * * * * *";

	public async Task ExecuteJob()
	{
		using var scope = _scopeFactory.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
		

		var maxDuration = DateTime.UtcNow.AddDays(-30);

		var unusedList = await context.Hotels
			.Where(r => r.IsDeleted && r.LastModified.UtcDateTime < maxDuration)
			.ToListAsync();

		var unusedIdList = unusedList.Select(h => h.Id).ToList();
		if (!unusedList.Any())
		{
			_logger.LogInformation("No unused hotel data to delete.");
			return;
		}

		_logger.LogInformation($"Processing {unusedList.Count} unused hotels deleted more than 30 days ago.");

		//singleton task cant use scope DI ApplicationDbContext
		context.Hotels.RemoveRange(unusedList);

		await context.SaveChangesAsync(CancellationToken.None);

		_logger.LogInformation("Deleted unused hotel data successfully.");
	}
}
