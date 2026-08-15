using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.PageDtos;

namespace Rentaly.WebUI.Controllers;

public class AdminLayoutController : Controller
{
    private readonly IHomeStatisticsService _statisticsService;

    public AdminLayoutController(IHomeStatisticsService statisticsService) =>
        _statisticsService = statisticsService;

    [HttpGet("/admin")]
    public async Task<IActionResult> Index()
    {
        var statistics = await _statisticsService.TGetAsync();
        return View("Dashboard", new AdminDashboardDto
        {
            ActiveCarCount = statistics.ActiveCarCount,
            AvailableCarCount = statistics.AvailableCarCount,
            TotalRentalCount = statistics.TotalRentalCount,
            PendingRentalCount = statistics.PendingRentalCount,
            CompletedRentalCount = statistics.CompletedRentalCount,
            CustomerCount = statistics.CustomerCount,
            TestimonialCount = statistics.TestimonialCount,
            BranchCount = statistics.BranchCount
        });
    }
}
