using Microsoft.Extensions.Logging;
using Scheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Tasks;
internal class DemoTask : ISchedulerTask
{
	private readonly ILogger<DemoTask> _logger;

	public DemoTask(ILogger<DemoTask> logger)
	{
		_logger = logger;
	}

	public string Name => nameof(DemoTask);

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
