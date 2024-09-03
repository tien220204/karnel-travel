using KarnelTravel.Application.Features.MasterData.Commands;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.MasterData;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/location")]
public class LocationController : ApiControllerBase
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("import-file")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ImportLocationFile(ImportLocationDataCommand command)
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
