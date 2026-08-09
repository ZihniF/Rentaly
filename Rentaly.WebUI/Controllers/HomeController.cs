using Microsoft.AspNetCore.Mvc;
using Rentaly.WebUI.Models;
using System.Diagnostics;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _cars;
        private readonly IBrandService _brands;
        private readonly ICarModelService _models;
        private readonly IBranchService _branches;
        private readonly IHomeContentService _contents;

        public HomeController(ICarService cars, IBrandService brands, ICarModelService models,
            IBranchService branches, IHomeContentService contents)
        {
            _cars = cars; _brands = brands; _models = models; _branches = branches; _contents = contents;
        }

        public async Task<IActionResult> Index(int? branchId, DateTime? pickupDate, DateTime? returnDate)
        {
            var hasBothDates = pickupDate.HasValue && returnDate.HasValue;
            var hasOnlyOneDate = pickupDate.HasValue != returnDate.HasValue;
            var hasValidRange = hasBothDates && returnDate > pickupDate;

            if (hasOnlyOneDate)
                ModelState.AddModelError(string.Empty, "Müsaitlik araması için alış ve iade tarihlerini birlikte seçmelisiniz.");
            else if (hasBothDates && !hasValidRange)
                ModelState.AddModelError(string.Empty, "İade tarihi teslim alma tarihinden sonra olmalıdır.");

            var contents = await _contents.TGetActiveAsync();
            var filter = new CarFilterDto
            {
                BranchId = branchId,
                PickupDate = hasValidRange ? pickupDate : null,
                ReturnDate = hasValidRange ? returnDate : null
            };

            var model = new HomeViewModel
            {
                Processes = contents.Where(x => x.Section == HomeSectionType.Process).ToList(),
                Futures = contents.Where(x => x.Section == HomeSectionType.Future).ToList(),
                Statistics = contents.Where(x => x.Section == HomeSectionType.Statistic).ToList(),
                Awards = contents.Where(x => x.Section == HomeSectionType.Award).ToList(),
                Testimonials = contents.Where(x => x.Section == HomeSectionType.Testimonial).ToList(),
                Faqs = contents.Where(x => x.Section == HomeSectionType.Faq).ToList(),
                Cars = await _cars.TGetFilteredCarsAsync(filter, 10),
                Brands = await _brands.TGetWithActiveCarsAsync(),
                Models = await _models.TGetWithActiveCarsAsync(),
                Branches = (await _branches.TGetListAsync()).OrderBy(x => x.City).ThenBy(x => x.BranchName).ToList(),
                SelectedBranchId = branchId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                AvailabilitySearchApplied = branchId.HasValue && hasValidRange
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("404")]
        public IActionResult NotFoundPage() { Response.StatusCode = 404; return View("NotFound"); }
    }
}
