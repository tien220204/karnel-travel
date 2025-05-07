using KarnelTravel.Application.Feature.Hotels;
using KarnelTravel.Application.Features.MasterData.Commands;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.MasterData;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/amenity")]
public class AmenityController : ApiControllerBase
{
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateAmenityAsync(CreateAmenityCommand command)
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
