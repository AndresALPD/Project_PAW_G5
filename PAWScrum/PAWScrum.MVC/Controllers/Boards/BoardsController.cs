using Microsoft.AspNetCore.Mvc;

namespace PAWScrum.Mvc.Controllers
{
    public class BoardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
