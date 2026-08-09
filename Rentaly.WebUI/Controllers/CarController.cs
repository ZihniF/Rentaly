using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers;

public class CarController : Controller
{
    private readonly ICarService _carService;
    private readonly ICategoryService _categoryService;
    private readonly IBranchService _branchService;
    private readonly IBrandService _brandService;
    private readonly ICarModelService _modelService;

    public CarController(ICarService carService, ICategoryService categoryService,
        IBranchService branchService, IBrandService brandService, ICarModelService modelService)
        => (_carService, _categoryService, _branchService, _brandService, _modelService) =
            (carService, categoryService, branchService, brandService, modelService);

    public async Task<IActionResult> CarList() => View(await _carService.TGetAllCarsWithCategoryAsync());
    public Task<IActionResult> List() => CarList();

    public async Task<IActionResult> Detail(int id)
    {
        var car = await _carService.TGetCarWithDetailsAsync(id);
        return car is null ? NotFound() : View(car);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCar()
    {
        await FillSelectionsAsync(new Car());
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCar(Car car)
    {
        if (!ModelState.IsValid)
        {
            await FillSelectionsAsync(car);
            return View(car);
        }

        await _carService.TInsertAsync(car);
        TempData["Success"] = "Araç eklendi.";
        return RedirectToAction(nameof(CarList));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _carService.TGetCarWithDetailsAsync(id);
        if (car is null) return NotFound();
        await FillSelectionsAsync(car);
        return View("CreateCar", car);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Car car)
    {
        if (!ModelState.IsValid)
        {
            await FillSelectionsAsync(car);
            return View("CreateCar", car);
        }

        await _carService.TUpdateAsync(car);
        TempData["Success"] = "Araç güncellendi.";
        return RedirectToAction(nameof(CarList));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _carService.TDeleteAsync(id); TempData["Success"] = "Araç silindi."; }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(CarList));
    }

    private async Task FillSelectionsAsync(Car car)
    {
        ViewBag.Categories = new SelectList(await _categoryService.TGetListAsync(), "CategoryId", "CategoryName", car.CategoryId);
        ViewBag.Brands = new SelectList(await _brandService.TGetListAsync(), "BrandId", "BrandName", car.BrandId);
        var models = await _modelService.TGetAllWithBrandAsync();
        ViewBag.Models = models.Select(x => new SelectListItem
        {
            Value = x.CarModelId.ToString(),
            Text = $"{x.Brand?.BrandName} {x.ModelName}",
            Selected = x.CarModelId == car.ModelId
        }).ToList();
        ViewBag.Branches = new SelectList(await _branchService.TGetListAsync(), "BranchId", "BranchName", car.BranchId);
    }
}
