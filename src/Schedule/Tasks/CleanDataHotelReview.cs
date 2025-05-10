using KarnelTravel.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Scheduler.Tasks;
public class CleanDataHotelReview
{
	private readonly ILogger<CleanDataHotelReview> _logger;
	//private readonly IElasticSearchService _elasticSearchService;
	private readonly IApplicationDbContext _context;

	public CleanDataHotelReview(
		ILogger<CleanDataHotelReview> logger,
		IApplicationDbContext context)
	{
		_logger = logger;
		_context = context;

	}
	public string Name => nameof(ImportProcessingEventTask);

	/// <summary>
	/// Run every 30 seconds
	/// </summary>
	public string CronTime => "*/30 * * * * *";
	public async Task ExecuteJob()
	{
		var maxDuration = DateTime.UtcNow.AddDays(-30);

		var unusedList = await _context.HotelReviews.Where(r => r.IsDeleted && r.LastModified.UtcDateTime < maxDuration ).ToListAsync();

		if(unusedList.IsNullOrEmpty())
		{
			_logger.LogInformation("No unused data delete");
			return;
		}

		_logger.LogInformation($"Processing {unusedList.Count} unused reviews in 30 days");

		_context.HotelReviews.RemoveRange(unusedList);

		_logger.LogInformation("remove unused review successfully");

		await Task.CompletedTask;
	}
}
