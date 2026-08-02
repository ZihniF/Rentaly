using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    public interface ICategoryController
    {
        IActionResult CategoryList();
    }
}