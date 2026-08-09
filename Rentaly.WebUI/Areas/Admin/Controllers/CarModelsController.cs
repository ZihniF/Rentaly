using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CarModelsController : Controller
{
    private readonly ICarModelService _models; private readonly IBrandService _brands;
    public CarModelsController(ICarModelService models, IBrandService brands) => (_models, _brands) = (models, brands);
    public async Task<IActionResult> Index() => View(await _models.TGetAllWithBrandAsync());
    [HttpGet] public async Task<IActionResult> Create() { await Brands(); return View("Form", new CarModel()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(CarModel model)
    { if (!ModelState.IsValid) { await Brands(); return View("Form", model); } await _models.TInsertAsync(model); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id) { var model=await _models.TGetByIdAsync(id); await Brands(model.BrandId); return View("Form", model); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(CarModel model)
    { if (!ModelState.IsValid) { await Brands(model.BrandId); return View("Form", model); } await _models.TUpdateAsync(model); return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await _models.TDeleteAsync(id); return RedirectToAction(nameof(Index)); }
    private async Task Brands(int? selected=null) => ViewBag.Brands = new SelectList(await _brands.TGetListAsync(), "BrandId", "BrandName", selected);
}
