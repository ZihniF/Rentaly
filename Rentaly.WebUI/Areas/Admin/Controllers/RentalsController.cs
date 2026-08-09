using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Enums;
using Rentaly.WebUI.Services;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class RentalsController : Controller
{
    private readonly IRentalService _rentals; private readonly IReservationEmailService _email;
    public RentalsController(IRentalService rentals, IReservationEmailService email) => (_rentals, _email) = (rentals, email);
    public async Task<IActionResult> Index() => View(await _rentals.TGetListAsync());

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
                TempData["Success"] = "Rezervasyon durumu güncellendi.";
            }
        }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
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
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
