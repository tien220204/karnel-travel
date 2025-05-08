using KarnelTravel.Application.Features.Hotels.Commands.HotelReview;
using KarnelTravel.Application.Features.Hotels.Commands.HoteRoom;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/hotel/room")]
public class HotelRoomController : ApiControllerBase
{
	/// <summary>
	/// Create room for hotel async
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateHotelRoomAsync(CreateHotelRoomCommand command)
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
	/// Update hotel room by id 
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("Update")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateHotelRoomAsync(UpdateHotelRoomCommand command)
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
	/// delete room by id
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("delete")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteHoteRoomAsync(DeleteHotelRoomCommand command)
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
