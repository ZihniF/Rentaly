using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.Controllers
{
    public class AdminLayoutController : Controller
    {
        [HttpGet("/admin")]
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}
