using KarnelTravel.Application.Common.Models;
using KarnelTravel.Application.Feature.Hotels;
using KarnelTravel.Application.Features.Flights.Commands;
using KarnelTravel.Application.Features.Flights.Models.Dtos;
using KarnelTravel.Application.Features.Flights.Queries;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Flight;



[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/flight")]
public class FlightController : ApiControllerBase
{
	/// <summary>
	/// create flight command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateFlightAsync(CreateHotelCommand command)
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
	/// Get flight with filter and pagination query
	/// </summary>
	/// <param name="query"></param>
	/// <returns></returns>
	[HttpPost("search")]
	[ProducesResponseType(typeof(AppApiResult<PaginatedList<FlightDto>>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<PaginatedList<FlightDto>>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetFlightWithSearchFilterAndPaginationQueryAsync(GetFlightsWithFilterAndPaginationQuery query)
	{
		if (!ModelState.IsValid)
		{
			return ClientError(ModelState);
		}

		var result = await Mediator.Send(query);

		if (result.IsSuccess)
		{
			return Success(result.Data, result.Detail);
		}

		return ClientError(result.Detail);
	}

	/// <summary>
	/// Update flight command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("update")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateFlightAsync(UpdateFlightCommand command)
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

	[HttpPost("delete")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteFlightAsync(DeleteFlightCommand command)
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