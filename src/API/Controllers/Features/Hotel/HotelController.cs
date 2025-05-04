using KarnelTravel.Application.Common.Models;
using KarnelTravel.Application.Common.Security;
using KarnelTravel.Application.Feature.Hotels;
using KarnelTravel.Application.Features.Hotels.Models.Dtos;
using KarnelTravel.Application.Features.Hotels.Queries;
using KarnelTravel.Application.Features.MasterData.Commands;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/hotel")]
public class HotelController : ApiControllerBase
{
	/// <summary>
	/// create hotel command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateHotelAsync(CreateHotelCommand command)
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
	/// Get hotel with filter and pagination query
	/// </summary>
	/// <param name="query"></param>
	/// <returns></returns>
	[HttpPost("search")]
	[ProducesResponseType(typeof(AppApiResult<PaginatedList<HotelDto>>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<PaginatedList<HotelDto>>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetHotelWithSearchFilterAndPaginationQueryAsync(GetHotelWithFilterAndPaginationQuery query)
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
}
