using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Tasks;
public class ImportProcessingEventTask : ISchedulerTask
{
	private readonly ILogger<ImportProcessingEventTask> _logger;
	//private readonly AppSettings _appSettings;
	public ImportProcessingEventTask(
		//IOptions<AppSettings> appSettingsAccessor,
		ILogger<ImportProcessingEventTask> logger)
	{
		_logger = logger;
		//_appSettings = appSettingsAccessor.Value;
	}
	public string Name => nameof(ImportProcessingEventTask);

	/// <summary>
	/// Run every 30 seconds
	/// </summary>
	public string CronTime => "*/30 * * * * *";
	public async Task ExecuteJob()
	{
		//StringContent httpContent = new(string.Empty, System.Text.Encoding.UTF8, "application/json");
		//var response = await _httpClient.PostAsync(Url, httpContent);
		//response.EnsureSuccessStatusCode();
		//_logger.LogInformation($"ExecuteJob of {Name} running with response {response.ReasonPhrase} and status code {response.StatusCode}");
		await Task.CompletedTask;
	}
}
