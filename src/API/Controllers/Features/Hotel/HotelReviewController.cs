using KarnelTravel.Application.Features.Hotels.Commands.HotelReview;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Application.Features.Hotels.Queries;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/hotel-review")]
public class HotelReviewController : ApiControllerBase
{
	/// <summary>
	/// create hotel review command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateHotelReviewAsync(CreateHotelReviewCommand command)
	{

		if (!ModelState.IsValid)
		{
			return ClientError(ModelState);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			return Success(result.Data);
		}

		return ClientError(result.Detail);
	}

	/// <summary>
	/// update hotel review by id command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("update")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateHotelReviewAsync(UpdateHotelReviewCommand command)
	{

		if (!ModelState.IsValid)
		{
			return ClientError(ModelState);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			return Success(result.Data);
		}

		return ClientError(result.Detail);
	}

	/// <summary>
	/// delete hotel review by id command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("delete")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteHotelAsync(DeleteHotelReviewCommand command)
	{

		if (!ModelState.IsValid)
		{
			return ClientError(ModelState);
		}

		var result = await Mediator.Send(command);

		if (result.IsSuccess)
		{
			return Success(result.Data);
		}

		return ClientError(result.Detail);
	}


	[HttpGet("all")]
	[ProducesResponseType(typeof(AppApiResult<IList<HotelReviewDto>>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<IList<HotelReviewDto>>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetAllProductCategoryAsync()
	{
		var result = await Mediator.Send(new GetAllHotelReviewQuery { });
		if (result.IsSuccess)
		{
			return Success(result.Data, result.Detail);
		}

		return ClientError(result.Detail);
	}
}
