using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.WebUI.ViewComponents;

public class AdminNotificationViewComponent : ViewComponent
{
    private readonly IRentalService _rentalService;

    public AdminNotificationViewComponent(IRentalService rentalService) => _rentalService = rentalService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var pendingRentals = (await _rentalService.TGetListAsync())
            .Where(x => x.Status == RentalStatus.Pending)
            .OrderByDescending(x => x.RentalId)
            .ToList();

        return View(new AdminNotificationDto
        {
            PendingCount = pendingRentals.Count,
            LatestPendingRentals = pendingRentals.Take(5).ToList()
        });
    }
}
