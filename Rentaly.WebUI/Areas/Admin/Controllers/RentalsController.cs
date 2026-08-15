using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Enums;
using Rentaly.WebUI.Mappings;
using Rentaly.WebUI.Services;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class RentalsController : Controller
{
    private readonly IRentalService _rentals;
    private readonly ICarService _cars;
    private readonly ICustomerService _customers;
    private readonly IBranchService _branches;
    private readonly IReservationEmailService _email;

    public RentalsController(
        IRentalService rentals,
        ICarService cars,
        ICustomerService customers,
        IBranchService branches,
        IReservationEmailService email)
        => (_rentals, _cars, _customers, _branches, _email) =
            (rentals, cars, customers, branches, email);

    public async Task<IActionResult> Index() => View(await _rentals.TGetListAsync());

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new AdminRentalFormDto
        {
            PickupDate = DateTime.Today.AddDays(1).AddHours(10),
            ReturnDate = DateTime.Today.AddDays(2).AddHours(10)
        };
        await FillFormAsync(model);
        return View("Form", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminRentalFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await FillFormAsync(model);
            return View("Form", model);
        }

        try
        {
            await _rentals.TCreateAsync(new CreateRentalDto
            {
                CarId = model.CarId,
                CustomerId = model.CustomerId,
                PickupBranchId = model.PickupBranchId,
                ReturnBranchId = model.ReturnBranchId,
                PickupDate = model.PickupDate,
                ReturnDate = model.ReturnDate
            });
            TempData["Success"] = "Rezervasyon eklendi ve tarih aralığı kilitlendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await FillFormAsync(model);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var rental = await _rentals.TGetByIdAsync(id);
        if (rental is null) return NotFound();
        if (rental.Status != RentalStatus.Pending)
        {
            TempData["Error"] = "Yalnızca bekleyen rezervasyonlar düzenlenebilir.";
            return RedirectToAction(nameof(Index));
        }

        var model = new AdminRentalFormDto
        {
            RentalId = rental.RentalId,
            CarId = rental.CarId,
            CustomerId = rental.CustomerId,
            PickupBranchId = rental.PickupBranchId,
            ReturnBranchId = rental.ReturnBranchId,
            PickupDate = rental.PickupDate,
            ReturnDate = rental.ReturnDate
        };
        await FillFormAsync(model);
        return View("Form", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminRentalFormDto model)
    {
        if (!ModelState.IsValid)
        {
            await FillFormAsync(model);
            return View("Form", model);
        }

        try
        {
            await _rentals.TUpdateAsync(new UpdateRentalDto
            {
                RentalId = model.RentalId,
                CarId = model.CarId,
                CustomerId = model.CustomerId,
                PickupBranchId = model.PickupBranchId,
                ReturnBranchId = model.ReturnBranchId,
                PickupDate = model.PickupDate,
                ReturnDate = model.ReturnDate
            });
            TempData["Success"] = "Rezervasyon güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await FillFormAsync(model);
            return View("Form", model);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, RentalStatus status)
    {
        try
        {
            await _rentals.TUpdateStatusAsync(new UpdateRentalStatusDto { RentalId = id, Status = status });
            if (status == RentalStatus.Approved)
            {
                var rental = (await _rentals.TGetListAsync()).First(x => x.RentalId == id);
                var sent = await _email.SendApprovalAsync(rental);
                TempData[sent ? "Success" : "Warning"] = sent
                    ? "Rezervasyon onaylandı ve e-posta gönderildi."
                    : "Rezervasyon onaylandı; SMTP yapılandırılmadığı için e-posta gönderilemedi.";
            }
            else
            {
                TempData["Success"] = "Rezervasyon reddedildi; tarih aralığı yeniden müsait.";
            }
        }
        catch (Exception exception)
        {
            TempData["Error"] = exception.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendApprovalEmail(int id)
    {
        try
        {
            var rental = (await _rentals.TGetListAsync()).FirstOrDefault(x => x.RentalId == id);
            if (rental is null) throw new InvalidOperationException("Rezervasyon bulunamadı.");
            if (rental.Status != RentalStatus.Approved)
                throw new InvalidOperationException("Onay e-postası yalnızca onaylı rezervasyonlara gönderilebilir.");

            var sent = await _email.SendApprovalAsync(rental);
            TempData[sent ? "Success" : "Warning"] = sent
                ? "Onay e-postası yeniden gönderildi."
                : "SMTP yapılandırılmadığı için e-posta gönderilemedi.";
        }
        catch (Exception exception)
        {
            TempData["Error"] = exception.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _rentals.TDeleteAsync(id);
            TempData["Success"] = "Rezervasyon silindi; ilgili tarih aralığı yeniden müsait hale geldi.";
        }
        catch (Exception exception)
        {
            TempData["Error"] = exception.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task FillFormAsync(AdminRentalFormDto model)
    {
        model.Cars = (await _cars.TGetAllCarsWithCategoryAsync())
            .Where(x => x.IsActive && x.IsAvailable)
            .Select(x => x.ToCardDto())
            .OrderBy(x => x.BrandName)
            .ThenBy(x => x.ModelName)
            .ToList();
        model.Customers = (await _customers.TGetListAsync())
            .Select(x => new CustomerOptionDto
            {
                CustomerId = x.CustomerId,
                FullName = $"{x.Name} {x.Surname}",
                Email = x.Email
            })
            .OrderBy(x => x.FullName)
            .ToList();
        model.Branches = (await _branches.TGetListAsync())
            .OrderBy(x => x.City)
            .ThenBy(x => x.BranchName)
            .Select(x => x.ToOptionDto())
            .ToList();
    }
}
