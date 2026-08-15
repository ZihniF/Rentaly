using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CarModelsController : Controller
{
    private readonly ICarModelService _models;
    private readonly IBrandService _brands;

    public CarModelsController(ICarModelService models, IBrandService brands) =>
        (_models, _brands) = (models, brands);

    public IActionResult Index() => RedirectToAction("BrandList", "Brand", new { area = "" });

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await FillBrandsAsync();
        return View("Form", new CarModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarModel model)
    {
        if (!ModelState.IsValid)
        {
            await FillBrandsAsync();
            return View("Form", model);
        }

        await _models.TInsertAsync(model);
        TempData["Success"] = "Model eklendi.";
        return RedirectToBrandList();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _models.TGetByIdAsync(id);
            await FillBrandsAsync(model.BrandId);
            return View("Form", model);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CarModel model)
    {
        if (!ModelState.IsValid)
        {
            await FillBrandsAsync(model.BrandId);
            return View("Form", model);
        }

        await _models.TUpdateAsync(model);
        TempData["Success"] = "Model güncellendi.";
        return RedirectToBrandList();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _models.TDeleteAsync(id);
            TempData["Success"] = "Model silindi.";
        }
        catch
        {
            TempData["Error"] = "Modele bağlı araç bulunduğu için model silinemedi.";
        }
        return RedirectToBrandList();
    }

    private async Task FillBrandsAsync(int? selected = null) =>
        ViewBag.Brands = new SelectList(await _brands.TGetListAsync(), "BrandId", "BrandName", selected);

    private RedirectToActionResult RedirectToBrandList() =>
        RedirectToAction("BrandList", "Brand", new { area = "" });
}
