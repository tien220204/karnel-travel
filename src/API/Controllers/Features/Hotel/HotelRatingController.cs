using KarnelTravel.Application.Feature.Hotels;
using KarnelTravel.Application.Features.Hotels.Commands.HotelRating;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/hotel-rating")]
public class HotelRatingController : ApiControllerBase
{
	/// <summary>
	/// create hotel rating command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateHotelAsync(CreateHotelRatingCommand command)
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
	/// update hotel rating by id command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("update")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateHotelRatingAsync(UpdateHotelRatingCommand command)
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
	/// Delete hotel rating command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("delete")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteHotelRatingAsync(DeleteHotelRatingCommand command)
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
}
