using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarnelTravel.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : BaseController
{
	private ISender _mediator = null!;

	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

	protected string GetLoggedInUserName()
	{
		if (User.Identity == null)
			throw new ArgumentNullException();

		return User.Identity.Name ?? "system";
	}

	protected string GetLoggedInUserId()
	{
		if (User.Identity == null || User.Identity.Name == null)
			throw new ArgumentNullException();

		return User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
