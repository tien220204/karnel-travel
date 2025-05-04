using KarnelTravel.Application.Features.ApplicationUsers.Commands;
using KarnelTravel.Application.Features.Hotels.Commands.HotelRating;
using KarnelTravel.Share.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.ApplicationUser;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user")]
public class ApplicationUserController : ApiControllerBase
{
	/// <summary>
	/// create user account command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create")]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(AppApiResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateUserAccountAsync(CreateApplicationUserCommand command)
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
