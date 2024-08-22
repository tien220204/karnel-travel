using System.Diagnostics;

namespace KarnelTravel.API.Logging;

/// <summary>
/// PerformanceTimerLogger
/// </summary>
public class PerformanceTimerLogger<T> : IDisposable
{
	private readonly ILogger<T> _logger;
	private readonly string _className;
	private readonly string _methodName;
	private readonly Stopwatch _timer;
	private readonly string _message;
	private readonly string _invoker;

	/// <summary>
	/// PerformanceTimerLogger
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="className"></param>
	/// <param name="methodName"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public PerformanceTimerLogger(ILogger<T> logger, string className, string methodName, string message, string invoker = null)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_className = className;
		_methodName = methodName;
		_message = message;
		_invoker = invoker;
		_timer = new Stopwatch();
		_timer.Start();
	}

	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose
	/// </summary>
	/// <param name="disposing"></param>
	protected virtual void Dispose(bool disposing)
	{
		_timer.Stop();

		if (_timer.ElapsedMilliseconds > 2000)
		{
			_logger.LogWarning("Long Running Request: {ClassName}.{MethodName} ({ElapsedMilliseconds} milliseconds) {Message} {@Invoker}",
				 _className, _methodName, _timer.ElapsedMilliseconds, _message, _invoker);
		}
	}
}