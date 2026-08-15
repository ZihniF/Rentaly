using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CarDtos;
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

    public async Task<IActionResult> CarList() => View("CarList", await _carService.TGetAllCarsWithCategoryAsync());
    public Task<IActionResult> List() => CarList();

    public async Task<IActionResult> Detail(int id)
    {
        var car = await _carService.TGetCarWithDetailsAsync(id);
        return car is null ? NotFound() : View(car);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCar()
    {
        var model = new AdminCarFormDto();
        await FillSelectionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCar(AdminCarFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await FillSelectionsAsync(model);
            return View(model);
        }

        try
        {
            await _carService.TInsertAsync(ToEntity(model));
            TempData["Success"] = "Araç eklendi.";
            return RedirectToAction(nameof(CarList));
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception is ValidationException
                ? exception.Message
                : "Araç kaydedilemedi. Plaka ve şasi numarasının benzersiz olduğunu kontrol edin.");
            await FillSelectionsAsync(model);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _carService.TGetCarWithDetailsAsync(id);
        if (car is null) return NotFound();

        var model = ToDto(car);
        await FillSelectionsAsync(model);
        return View("CreateCar", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminCarFormDto model)
    {
        if (model.CarId <= 0) return BadRequest();

        if (!ModelState.IsValid)
        {
            await FillSelectionsAsync(model);
            return View("CreateCar", model);
        }

        try
        {
            var existingCar = await _carService.TGetByIdAsync(model.CarId);
            Apply(model, existingCar);
            await _carService.TUpdateAsync(existingCar);
            TempData["Success"] = "Araç güncellendi.";
            return RedirectToAction(nameof(CarList));
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception is ValidationException
                ? exception.Message
                : "Araç güncellenemedi. Plaka ve şasi numarasının benzersiz olduğunu kontrol edin.");
            await FillSelectionsAsync(model);
            return View("CreateCar", model);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _carService.TDeleteAsync(id);
            TempData["Success"] = "Araç silindi.";
        }
        catch
        {
            TempData["Error"] = "Araç rezervasyon geçmişinde kullanıldığı için silinemedi. Geçmiş kayıtları korumak için aracı düzenleyip pasif duruma getirebilirsiniz.";
        }

        return RedirectToAction(nameof(CarList));
    }

    private async Task FillSelectionsAsync(AdminCarFormDto model)
    {
        ViewBag.Categories = new SelectList(await _categoryService.TGetListAsync(), "CategoryId", "CategoryName", model.CategoryId);
        ViewBag.Brands = new SelectList(await _brandService.TGetListAsync(), "BrandId", "BrandName", model.BrandId);
        ViewBag.CarModels = await _modelService.TGetAllWithBrandAsync();
        ViewBag.Branches = new SelectList(await _branchService.TGetListAsync(), "BranchId", "BranchName", model.BranchId);
    }

    private static AdminCarFormDto ToDto(Car car) => new()
    {
        CarId = car.CarId,
        PlateNumber = car.PlateNumber,
        VIN = car.VIN,
        BrandId = car.BrandId,
        ModelId = car.ModelId,
        CategoryId = car.CategoryId,
        BranchId = car.BranchId,
        Year = car.Year,
        Kilometer = car.Kilometer,
        DailyPrice = car.DailyPrice,
        DepositAmount = car.DepositAmount,
        IsAvailable = car.IsAvailable,
        IsActive = car.IsActive,
        ImageUrl = car.ImageUrl,
        SeatCount = car.SeatCount,
        LuggageCount = car.LuggageCount,
        FuelType = car.FuelType
    };

    private static Car ToEntity(AdminCarFormDto model)
    {
        var car = new Car();
        Apply(model, car);
        return car;
    }

    private static void Apply(AdminCarFormDto model, Car car)
    {
        car.PlateNumber = model.PlateNumber.Trim().ToUpperInvariant();
        car.VIN = model.VIN.Trim().ToUpperInvariant();
        car.BrandId = model.BrandId;
        car.ModelId = model.ModelId;
        car.CategoryId = model.CategoryId;
        car.BranchId = model.BranchId;
        car.Year = model.Year;
        car.Kilometer = model.Kilometer;
        car.DailyPrice = model.DailyPrice;
        car.DepositAmount = model.DepositAmount;
        car.IsAvailable = model.IsAvailable;
        car.IsActive = model.IsActive;
        car.ImageUrl = model.ImageUrl.Trim();
        car.SeatCount = model.SeatCount;
        car.LuggageCount = model.LuggageCount;
        car.FuelType = model.FuelType.Trim();
    }
}
