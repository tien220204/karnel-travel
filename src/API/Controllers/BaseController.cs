using KarnelTravel.Share.Common.Enums;
using KarnelTravel.Share.Common.Exceptions;
using KarnelTravel.Share.Common.Helpers;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace KarnelTravel.API.Controllers;

/// <summary>
/// Base controller with helpers (non-secured)
/// </summary>
/// <seealso cref="Controller" />
[ApiController]
public abstract class BaseController : Controller
{
	#region 1. Api Action Result

	#region 1.1 Success

	/// <summary>
	/// Return API success result (HttpCode: 200) with data (if any)
	/// </summary>
	/// <param name="value">The content value to format in the entity body.</param>
	/// <returns>OK: HttpCode 200 with data (if any)</returns>
	public override OkObjectResult Ok(object value) { return Success(value); }

	/// <summary>
	/// Return API success result (HttpCode: 200) with data (if any)
	/// </summary>
	/// <param name="value">The content value to format in the entity body.</param>
	/// <param name="message"></param>
	/// <returns>OK: HttpCode 200 with data (if any)</returns>
	protected OkObjectResult Success(object value, string message)
	{
		return Success(value, new AppMessage { Content = message, Type = AppMessageType.Success });
	}

	/// <summary>
	/// Return API success result (HttpCode: 200) with data (if any)
	/// </summary>
	/// <param name="data">data</param>
	/// <param name="message">message</param>
	/// <returns>OK: HttpCode 200 with data (if any)</returns>
	protected OkObjectResult Success(object data = null, AppMessage message = null)
	{
		var successResult = new AppApiResult(data)
		{
			IsSuccess = true
		};

		if (message == null)
			successResult.AddMessage("Successfully", AppMessageType.Success);
		else
			successResult.AddMessage(message.Content, message.Type);

		return base.Ok(successResult);
	}

	#endregion Success

	#region 1.2 Error

	// ***Summary Error***
	// 1. InternalError             -> 500
	// 2. BusinessError             -> 409 Conflict
	// 3. NotImplementedError       -> 501
	// 4. BadRequest                -> 400
	// 5. Authentication Error      -> 401
	// 6. Authorization Error       -> 403
	// 7. NotFound                  -> 404

	#region 1.2.1 Call Directly On Api

	/// <summary>
	/// Return API error with a message (has id and content)
	/// </summary>
	/// <param name="modelState">model state</param>
	/// <returns></returns>
	protected IActionResult ClientError(ModelStateDictionary modelState)
	{
		var errorBuilder = new StringBuilder();
		var errors = new List<AppMessage>();
		foreach (var val in modelState)
		{
			foreach (var err in val.Value.Errors)
			{
				errors.Add(new AppMessage
				{
					Type = AppMessageType.Error,
					Content = err.ErrorMessage
				});
				errorBuilder.AppendLine(err.ErrorMessage);
			}
		}
		var apiResult = new AppApiResult
		{
			IsSuccess = false,
			Messages = errors
		};

		return BadRequest(apiResult);
	}

	/// <summary>
	/// Return a business error <br/>
	/// Call directly when you specified the type of error
	/// </summary>
	/// <param name="errorModel">model contains error detail</param>
	/// <returns></returns>
	protected IActionResult BusinessError(object errorModel)
	{
		return StatusCode(StatusCodes.Status409Conflict, errorModel);
	}

	/// <summary>
	/// Return API error with a message (has id and content)
	/// </summary>
	/// <param name="message">The message.</param>
	/// <returns> IActionResult(EcrApiResult) </returns>
	protected IActionResult Error(AppMessage message)
	{
		return Error(message, ApiErrorType.InternalServerError);
	}

	/// <summary>
	/// Return API error with a message
	/// </summary>
	/// <param name="messageContent">content of the message</param>
	/// <returns>IActionResult(EcrApiResult)</returns>
	protected IActionResult Error(string messageContent)
	{
		return Error(new AppMessage { Content = messageContent });
	}

	/// <summary>
	/// Returns not-implemented-error <br/>
	/// Call directly when you specified the type of error
	/// </summary>
	/// <returns>IActionResult (HttpCode: 501 - NotImplemented)</returns>
	protected IActionResult NotImplementedError()
	{
		return StatusCode(StatusCodes.Status501NotImplemented);
	}

	/// <summary>
	/// Returns internal-server-error (HttpCode: 500) <br/>
	/// Call directly when you specified the type of error
	/// </summary>
	/// <param name="errorDetail">details of error</param>
	/// <returns>IActionResult (HttpCode: 500)</returns>
	protected IActionResult InternalServerError(object errorDetail = null)
	{
		return StatusCode(StatusCodes.Status500InternalServerError, errorDetail);
	}

	#endregion

	#region 1.2.2 Handle Error base on Exception type

	/// <summary>
	/// Return an API error from an exception
	/// </summary>
	/// <param name="e">exception</param>
	/// <param name="data">The data.</param>
	/// <returns>
	/// IActionResult(EcrApiResult)
	/// </returns>
	protected IActionResult Error(Exception e, object data = null)
	{
		// Return with mesage code and content
		if (e is AppBusinessException sbpEx)
		{
			return Error(new AppMessage { Content = sbpEx.Message }, ApiErrorType.Business, data);
		}

		if (e is AppDataNotFoundException notFoundEx)
			return NotFoundError(notFoundEx.Message);

		if (e is AppArgumentException argNullEx)
			return BadRequest(argNullEx.Message);

		return Error(e.Message);
	}

	#endregion

	#region 1.2.3 Private error helper

	/// <summary>
	/// Return API error with a message (has id and content)
	/// </summary>
	/// <param name="message">message</param>
	/// <param name="errorType">indicate if error type is business</param>
	/// <param name="data">The data.</param>
	/// <returns>
	/// IActionResult(EcrApiResult)
	/// </returns>
	private IActionResult Error(AppMessage message, ApiErrorType errorType = ApiErrorType.InternalServerError, object data = null)
	{
		var errorModel = ApiResultHelper.BuildErrorApiResult(message.Content, data);

		if (errorType == ApiErrorType.ClientError)
			return BadRequest(errorModel);

		if (errorType == ApiErrorType.Business)
			return BusinessError(errorModel);

		// Force to InternalServerError for other type of message
		return InternalServerError(errorModel);
	}

	/// <summary>
	/// Return not found error
	/// </summary>
	/// <param name="msgContent">Content of the MSG.</param>
	/// <returns></returns>
	protected IActionResult NotFoundError(string msgContent)
	{
		return NotFound(ApiResultHelper.BuildErrorApiResult(msgContent));
	}

	/// <summary>
	/// Clients the error.
	/// </summary>
	/// <param name="msgContent">Content of the MSG.</param>
	/// <param name="data">data</param>
	/// <returns></returns>
	protected IActionResult ClientError(string msgContent, object data = null)
	{
		return BadRequest(ApiResultHelper.BuildErrorApiResult(msgContent, data));
	}

	#endregion

	#endregion 3.2 Error

	#endregion 1. Api Action Result
}
