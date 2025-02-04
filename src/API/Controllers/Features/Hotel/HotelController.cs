 using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;
public class HotelController : ApiControllerBase
{
	public IActionResult Index()
	{
		return View();
	}
}
