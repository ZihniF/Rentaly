using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers;

public class BookingController : Controller
{
    private readonly ICarService _cars; private readonly IBranchService _branches;
    private readonly IRentalService _rentals;
    public BookingController(ICarService cars, IBranchService branches, IRentalService rentals)
        => (_cars, _branches, _rentals) = (cars, branches, rentals);

    [HttpGet]
    public async Task<IActionResult> Index(int carId, DateTime? pickupDate, DateTime? returnDate, int? branchId)
    {
        var model = new BookingViewModel { CarId = carId, PickupBranchId = branchId ?? 0, ReturnBranchId = branchId ?? 0,
            PickupDate = pickupDate ?? DateTime.Today.AddDays(1), ReturnDate = returnDate ?? DateTime.Today.AddDays(2) };
        await FillAsync(model); return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BookingViewModel model)
    {
        if (!ModelState.IsValid) { await FillAsync(model); return View(model); }
        try
        {
            var rentalId = await _rentals.TCreateBookingAsync(new CreateBookingDto { CarId = model.CarId,
                PickupBranchId = model.PickupBranchId, ReturnBranchId = model.ReturnBranchId,
                PickupDate = model.PickupDate, ReturnDate = model.ReturnDate,
                Name = model.Name, Surname = model.Surname, Email = model.Email, Phone = model.Phone,
                IdentityNumber = model.IdentityNumber });
            TempData["RentalId"] = rentalId;
            return RedirectToAction(nameof(Success));
        }
        catch (Exception ex) { ModelState.AddModelError(string.Empty, ex.Message); await FillAsync(model); return View(model); }
    }

    public IActionResult Success() => View();
    private async Task FillAsync(BookingViewModel model) { model.Cars = await _cars.TGetAllCarsWithCategoryAsync(); model.Branches = await _branches.TGetListAsync(); }
}
