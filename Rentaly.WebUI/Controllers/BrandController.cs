using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers;

public class BrandController : Controller
{
    private readonly IBrandService _brandService;
    public BrandController(IBrandService brandService) => _brandService = brandService;

    public async Task<IActionResult> BrandList() => View(await _brandService.TGetListAsync());
    [HttpGet] public IActionResult CreateBrand() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBrand(Brand brand)
    {
        var result = new BrandValidator().Validate(brand);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(brand);
        }
        await _brandService.TInsertAsync(brand);
        return RedirectToAction(nameof(BrandList));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try { return View(await _brandService.TGetByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Brand brand)
    {
        if (!ModelState.IsValid) return View(brand);
        await _brandService.TUpdateAsync(brand);
        return RedirectToAction(nameof(BrandList));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _brandService.TDeleteAsync(id); TempData["Success"] = "Marka silindi."; }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(BrandList));
    }
}
