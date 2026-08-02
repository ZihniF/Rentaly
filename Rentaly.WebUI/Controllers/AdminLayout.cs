using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.Controllers
{
    public class AdminLayout : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
