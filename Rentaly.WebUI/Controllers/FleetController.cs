using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.WebUI.Mappings;

namespace Rentaly.WebUI.Controllers;

public class FleetController : Controller
{
    private readonly ICarService _cars; private readonly IBrandService _brands;
    private readonly ICarModelService _models; private readonly IBranchService _branches;
    public FleetController(ICarService cars, IBrandService brands, ICarModelService models, IBranchService branches)
        => (_cars, _brands, _models, _branches) = (cars, brands, models, branches);

    public async Task<IActionResult> Index([FromQuery] CarFilterDto filter)
    {
        var brands = await _brands.TGetWithActiveCarsAsync();
        var models = await _models.TGetWithActiveCarsAsync();
        var branches = (await _branches.TGetListAsync()).OrderBy(x => x.City).ThenBy(x => x.BranchName).ToList();
        var queryFilter = new CarFilterDto
        {
            BrandId = filter.BrandId,
            ModelId = filter.ModelId,
            BranchId = filter.BranchId,
            MinPrice = filter.MinPrice,
            MaxPrice = filter.MaxPrice,
            PickupDate = filter.PickupDate,
            ReturnDate = filter.ReturnDate
        };

        if (filter.BrandId.HasValue && brands.All(x => x.BrandId != filter.BrandId.Value))
        {
            ModelState.AddModelError(string.Empty, "Seçilen marka bulunamadı.");
            queryFilter.BrandId = null;
        }
        if (filter.ModelId.HasValue && models.All(x => x.CarModelId != filter.ModelId.Value))
        {
            ModelState.AddModelError(string.Empty, "Seçilen model bulunamadı.");
            queryFilter.ModelId = null;
        }
        if (filter.BrandId.HasValue && filter.ModelId.HasValue &&
            models.Any(x => x.CarModelId == filter.ModelId.Value && x.BrandId != filter.BrandId.Value))
        {
            ModelState.AddModelError(string.Empty, "Seçilen model seçilen markaya ait değil.");
            queryFilter.ModelId = null;
        }
        if (filter.BranchId.HasValue && branches.All(x => x.BranchId != filter.BranchId.Value))
        {
            ModelState.AddModelError(string.Empty, "Seçilen lokasyon bulunamadı.");
            queryFilter.BranchId = null;
        }
        if (filter.MinPrice < 0 || filter.MaxPrice < 0)
        {
            ModelState.AddModelError(string.Empty, "Fiyat değerleri negatif olamaz.");
            queryFilter.MinPrice = null;
            queryFilter.MaxPrice = null;
        }
        else if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
        {
            ModelState.AddModelError(string.Empty, "Minimum fiyat maksimum fiyattan büyük olamaz.");
            queryFilter.MinPrice = null;
            queryFilter.MaxPrice = null;
        }

        var onlyOneDate = filter.PickupDate.HasValue != filter.ReturnDate.HasValue;
        if (onlyOneDate)
        {
            ModelState.AddModelError(string.Empty, "Müsaitlik için alış ve iade tarihlerini birlikte seçmelisiniz.");
            queryFilter.PickupDate = null;
            queryFilter.ReturnDate = null;
        }
        else if (filter.PickupDate.HasValue && filter.PickupDate.Value < DateTime.Now.AddMinutes(-1))
        {
            ModelState.AddModelError(string.Empty, "Alış tarihi geçmiş bir tarih olamaz.");
            queryFilter.PickupDate = null;
            queryFilter.ReturnDate = null;
        }
        else if (filter.PickupDate.HasValue && filter.ReturnDate <= filter.PickupDate)
        {
            ModelState.AddModelError(string.Empty, "İade tarihi alış tarihinden sonra olmalıdır.");
            queryFilter.PickupDate = null;
            queryFilter.ReturnDate = null;
        }

        var model = new FleetPageDto
        {
            Filter = filter,
            Cars = (await _cars.TGetFilteredCarsAsync(queryFilter)).Select(x => x.ToCardDto()).ToList(),
            Brands = brands.Select(x => x.ToOptionDto()).ToList(),
            Models = models.Select(x => x.ToOptionDto()).ToList(),
            Branches = branches.Select(x => x.ToOptionDto()).ToList()
        };
        return View(model);
    }
}
