using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DtoLayer.BrandDtos;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers;

public class BrandController : Controller
{
    private readonly IBrandService _brandService;
    private readonly ICarModelService _modelService;

    public BrandController(IBrandService brandService, ICarModelService modelService) =>
        (_brandService, _modelService) = (brandService, modelService);

    public async Task<IActionResult> BrandList()
    {
        var brands = await _brandService.TGetAllWithModelsAsync();
        var model = brands.Select(brand => new BrandManagementDto
        {
            BrandId = brand.BrandId,
            BrandName = brand.BrandName,
            ImageUrl = brand.ImageUrl,
            Models = brand.CarModels.Select(carModel => new BrandModelItemDto
            {
                CarModelId = carModel.CarModelId,
                ModelName = carModel.ModelName,
                VehicleCount = carModel.Cars.Count
            }).ToList()
        }).ToList();

        return View("BrandList", model);
    }

    [HttpGet]
    public IActionResult CreateBrand() => View();

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

        try
        {
            await _brandService.TInsertAsync(brand);
            TempData["Success"] = "Marka eklendi.";
            return RedirectToAction(nameof(BrandList));
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(brand);
        }
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
        try
        {
            var existing = await _brandService.TGetByIdAsync(brand.BrandId);
            existing.BrandName = brand.BrandName.Trim();
            existing.ImageUrl = brand.ImageUrl.Trim();
            await _brandService.TUpdateAsync(existing);
            TempData["Success"] = "Marka güncellendi.";
            return RedirectToAction(nameof(BrandList));
        }
        catch (ValidationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(brand);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _brandService.TDeleteAsync(id);
            TempData["Success"] = "Marka silindi.";
        }
        catch
        {
            TempData["Error"] = "Markaya bağlı model veya araç bulunduğu için marka silinemedi. Önce bağlı kayıtları kaldırın.";
        }
        return RedirectToAction(nameof(BrandList));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateModel(CreateBrandModelDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = FirstModelError();
            return RedirectToAction(nameof(BrandList), null, null, $"brand-{model.BrandId}");
        }

        try
        {
            await _brandService.TGetByIdAsync(model.BrandId);
            await _modelService.TInsertAsync(new CarModel
            {
                BrandId = model.BrandId,
                ModelName = model.ModelName
            });
            TempData["Success"] = "Model marka altına eklendi.";
        }
        catch (Exception exception)
        {
            TempData["Error"] = exception is ValidationException
                ? exception.Message
                : "Model eklenemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction(nameof(BrandList), null, null, $"brand-{model.BrandId}");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditModel(UpdateBrandModelDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = FirstModelError();
            return RedirectToAction(nameof(BrandList), null, null, $"brand-{model.BrandId}");
        }

        try
        {
            var existing = await _modelService.TGetByIdAsync(model.CarModelId);
            if (existing.BrandId != model.BrandId)
                throw new ValidationException("Model ve marka eşleşmiyor.");
            existing.ModelName = model.ModelName;
            await _modelService.TUpdateAsync(existing);
            TempData["Success"] = "Model güncellendi.";
        }
        catch (Exception exception)
        {
            TempData["Error"] = exception is ValidationException
                ? exception.Message
                : "Model güncellenemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction(nameof(BrandList), null, null, $"brand-{model.BrandId}");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteModel(int id, int brandId)
    {
        try
        {
            var existing = await _modelService.TGetByIdAsync(id);
            if (existing.BrandId != brandId)
                throw new ValidationException("Model ve marka eşleşmiyor.");
            await _modelService.TDeleteAsync(id);
            TempData["Success"] = "Model silindi.";
        }
        catch
        {
            TempData["Error"] = "Modele bağlı araç bulunduğu için model silinemedi. Önce araçların modelini değiştirin.";
        }

        return RedirectToAction(nameof(BrandList), null, null, $"brand-{brandId}");
    }

    private string FirstModelError() => ModelState.Values
        .SelectMany(value => value.Errors)
        .Select(error => error.ErrorMessage)
        .FirstOrDefault(message => !string.IsNullOrWhiteSpace(message))
        ?? "Form bilgilerini kontrol edin.";
}
