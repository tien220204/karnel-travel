

namespace KarnelTravel.Application.Common;
public abstract class BaseHandler
{
	/// <summary>
	/// Build multilingual error
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="errorMessage">error message</param>
	/// <returns></returns>
	protected static AppActionResultData<T> BuildMultilingualError<T>(AppActionResultData<T> result, string errorMessage)
	{
		errorMessage ??= string.Empty;
		result.BuildError(errorMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual error
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="errorMessage">error message</param>
	/// <param name="msgArguments">message arguments</param>
	/// <returns></returns>
	protected static AppActionResultData<T> BuildMultilingualError<T>(AppActionResultData<T> result, string errorMessage, params object[] msgArguments)
	{
		errorMessage ??= string.Empty;
		errorMessage = string.Format(errorMessage, msgArguments);
		result.BuildError(errorMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual error
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="errorMessage">error message</param>
	/// <returns></returns>
	protected static AppActionResult BuildMultilingualError(AppActionResult result, string errorMessage)
	{
		errorMessage ??= string.Empty;
		result.BuildError(errorMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual error
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="errorMessage">error message</param>
	/// <param name="msgArguments">message arguments</param>
	/// <returns></returns>
	protected static AppActionResult BuildMultilingualError(AppActionResult result, string errorMessage, params object[] msgArguments)
	{
		errorMessage ??= string.Empty;
		errorMessage = string.Format(errorMessage, msgArguments);
		result.BuildError(errorMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual result
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="data">data</param>
	/// <param name="resultMessage">result message</param>
	/// <returns></returns>
	protected static AppActionResultData<T> BuildMultilingualResult<T>(AppActionResultData<T> result, T data, string resultMessage)
	{
		resultMessage ??= string.Empty;
		result.BuildResult(data, resultMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual result
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="data">data</param>
	/// <param name="resultMessage">result message</param>
	/// <param name="msgArguments">message arguments</param>
	/// <returns></returns>
	protected static AppActionResultData<T> BuildMultilingualResult<T>(AppActionResultData<T> result, T data, string resultMessage, params object[] msgArguments)
	{
		resultMessage ??= string.Empty;
		resultMessage = string.Format(resultMessage, msgArguments);
		result.BuildResult(data, resultMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual result
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="resultMessage">result message</param>
	/// <returns></returns>
	protected static AppActionResult BuildMultilingualResultAsync(AppActionResult result, string resultMessage)
	{
		resultMessage ??= string.Empty;
		result.BuildResult(resultMessage);
		return result;
	}

	/// <summary>
	/// Build multilingual result
	/// </summary>
	/// <param name="result">result</param>
	/// <param name="resultMessage">result message</param>
	/// <param name="msgArguments">message arguments</param>
	/// <returns></returns>
	protected static AppActionResult BuildMultilingualResult(AppActionResult result, string resultMessage, params object[] msgArguments)
	{
		resultMessage ??= string.Empty;
		resultMessage = string.Format(resultMessage, msgArguments);
		result.BuildResult(resultMessage);
		return result;
	}
}
