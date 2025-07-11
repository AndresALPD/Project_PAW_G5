using Microsoft.AspNetCore.Mvc;

namespace PAWScrum.API.Controllers
{
	public class UsersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
