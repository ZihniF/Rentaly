using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.WebUI.Mappings;

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
        var selectedCar = carId > 0 ? await _cars.TGetCarWithDetailsAsync(carId) : null;
        if (carId > 0 && (selectedCar is null || !selectedCar.IsActive || !selectedCar.IsAvailable))
            return RedirectToAction("NotFoundPage", "Home");

        var effectivePickup = pickupDate ?? DateTime.Today.AddDays(1).AddHours(10);
        var effectiveReturn = returnDate ?? effectivePickup.AddDays(1);
        if (effectivePickup < DateTime.Now.AddMinutes(-1) || effectiveReturn <= effectivePickup)
        {
            ModelState.AddModelError(string.Empty, "Geçersiz tarih aralığı yerine varsayılan tarihler gösterildi.");
            effectivePickup = DateTime.Today.AddDays(1).AddHours(10);
            effectiveReturn = effectivePickup.AddDays(1);
        }

        var pickupBranchId = selectedCar?.BranchId ?? branchId ?? 0;
        var model = new BookingPageDto { CarId = carId, PickupBranchId = pickupBranchId, ReturnBranchId = branchId ?? pickupBranchId,
            PickupDate = effectivePickup, ReturnDate = effectiveReturn };
        await FillAsync(model); return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BookingPageDto model)
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
    private async Task FillAsync(BookingPageDto model)
    {
        var filter = new CarFilterDto
        {
            PickupDate = model.PickupDate >= DateTime.Now.AddMinutes(-1) && model.ReturnDate > model.PickupDate
                ? model.PickupDate : null,
            ReturnDate = model.PickupDate >= DateTime.Now.AddMinutes(-1) && model.ReturnDate > model.PickupDate
                ? model.ReturnDate : null
        };
        model.Cars = (await _cars.TGetFilteredCarsAsync(filter)).Select(x => x.ToCardDto()).ToList();
        model.Branches = (await _branches.TGetListAsync()).OrderBy(x => x.City).ThenBy(x => x.BranchName)
            .Select(x => x.ToOptionDto()).ToList();
    }
}
