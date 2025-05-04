using Microsoft.AspNetCore.Mvc;

namespace KarnelTravel.API.Controllers.Features.Hotel;
public class HotelRatingController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
