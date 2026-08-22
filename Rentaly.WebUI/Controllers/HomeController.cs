using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.EntityLayer.Enums;
using Rentaly.WebUI.Mappings;
using Rentaly.DtoLayer.HomeDtos;
using System.Globalization;

namespace Rentaly.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _cars;
        private readonly IBrandService _brands;
        private readonly ICarModelService _models;
        private readonly IBranchService _branches;
        private readonly IHomeContentService _contents;
        private readonly IHomeStatisticsService _statistics;
        private readonly IHomePageSettingsService _pageSettings;

        public HomeController(ICarService cars, IBrandService brands, ICarModelService models,
            IBranchService branches, IHomeContentService contents, IHomeStatisticsService statistics,
            IHomePageSettingsService pageSettings)
        {
            _cars = cars; _brands = brands; _models = models; _branches = branches;
            _contents = contents; _statistics = statistics;
            _pageSettings = pageSettings;
        }

        public async Task<IActionResult> Index(int? branchId, DateTime? pickupDate, DateTime? returnDate)
        {
            var branches = (await _branches.TGetListAsync())
                .OrderBy(x => x.City)
                .ThenBy(x => x.BranchName)
                .ToList();
            var searchRequested = branchId.HasValue || pickupDate.HasValue || returnDate.HasValue;
            var hasBothDates = pickupDate.HasValue && returnDate.HasValue;
            var branchExists = branchId.HasValue && branches.Any(x => x.BranchId == branchId.Value);
            var pickupIsCurrent = pickupDate.HasValue && pickupDate.Value >= DateTime.Now.AddMinutes(-1);
            var hasValidRange = hasBothDates && returnDate > pickupDate;
            var validAvailabilitySearch = searchRequested && branchExists && pickupIsCurrent && hasValidRange;

            if (searchRequested && (!branchId.HasValue || !hasBothDates))
                ModelState.AddModelError(string.Empty, "Müsaitlik araması için lokasyon, alış ve iade alanlarının tamamını seçmelisiniz.");
            else if (branchId.HasValue && !branchExists)
                ModelState.AddModelError(string.Empty, "Seçtiğiniz lokasyon bulunamadı.");
            else if (pickupDate.HasValue && !pickupIsCurrent)
                ModelState.AddModelError(string.Empty, "Alış tarihi geçmiş bir tarih olamaz.");
            else if (hasBothDates && !hasValidRange)
                ModelState.AddModelError(string.Empty, "İade tarihi teslim alma tarihinden sonra olmalıdır.");

            var contents = await _contents.TGetActiveAsync();
            var liveStatistics = await _statistics.TGetAsync();
            var statisticItems = contents
                .Where(x => x.Section == HomeSectionType.Statistic)
                .Select(x => x.ToPageDto())
                .ToList();
            foreach (var item in statisticItems)
                item.Subtitle = GetStatisticValue(item.Icon, item.DisplayOrder, liveStatistics);
            var filter = new CarFilterDto
            {
                BranchId = validAvailabilitySearch ? branchId : null,
                PickupDate = validAvailabilitySearch ? pickupDate : null,
                ReturnDate = validAvailabilitySearch ? returnDate : null
            };

            var model = new HomePageDto
            {
                Settings = await _pageSettings.TGetAsync(),
                Processes = contents.Where(x => x.Section == HomeSectionType.Process).Select(x => x.ToPageDto()).ToList(),
                Futures = contents.Where(x => x.Section == HomeSectionType.Future).Select(x => x.ToPageDto()).ToList(),
                Statistics = statisticItems,
                Awards = contents.Where(x => x.Section == HomeSectionType.Award).Select(x => x.ToPageDto()).ToList(),
                Testimonials = contents.Where(x => x.Section == HomeSectionType.Testimonial).Select(x => x.ToPageDto()).ToList(),
                Faqs = contents.Where(x => x.Section == HomeSectionType.Faq).Select(x => x.ToPageDto()).ToList(),
                Cars = (await _cars.TGetFilteredCarsAsync(filter, 10)).Select(x => x.ToCardDto()).ToList(),
                Brands = (await _brands.TGetWithActiveCarsAsync()).Select(x => x.ToOptionDto()).ToList(),
                Models = (await _models.TGetWithActiveCarsAsync()).Select(x => x.ToOptionDto()).ToList(),
                Branches = branches.Select(x => x.ToOptionDto()).ToList(),
                SelectedBranchId = branchId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                AvailabilitySearchApplied = validAvailabilitySearch
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorPageDto { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("404")]
        public IActionResult NotFoundPage() { Response.StatusCode = 404; return View("NotFound"); }

        private static string GetStatisticValue(string metricKey, int displayOrder, HomeStatisticsDto statistics)
        {
            var value = metricKey.Trim().ToLowerInvariant() switch
            {
                "completed-rentals" => statistics.CompletedRentalCount,
                "customers" => statistics.CustomerCount,
                "active-cars" => statistics.ActiveCarCount,
                "available-cars" => statistics.AvailableCarCount,
                "total-rentals" => statistics.TotalRentalCount,
                "pending-rentals" => statistics.PendingRentalCount,
                "testimonials" => statistics.TestimonialCount,
                "branches" => statistics.BranchCount,
                _ => displayOrder switch
                {
                    1 => statistics.CompletedRentalCount,
                    2 => statistics.CustomerCount,
                    3 => statistics.ActiveCarCount,
                    4 => statistics.BranchCount,
                    _ => 0
                }
            };

            return value.ToString("N0", CultureInfo.GetCultureInfo("tr-TR"));
        }
    }
}
