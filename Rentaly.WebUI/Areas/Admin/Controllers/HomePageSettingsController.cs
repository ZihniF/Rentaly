using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomePageSettingsController : Controller
{
    private readonly IHomePageSettingsService _service;

    public HomePageSettingsController(IHomePageSettingsService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _service.TGetAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(HomePageSettingsDto model)
    {
        if (!ModelState.IsValid) return View(model);
        await _service.TUpdateAsync(model);
        TempData["Success"] = "Ana sayfa başlıkları ve footer bilgileri güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}
