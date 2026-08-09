using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers;

public class FleetController : Controller
{
    private readonly ICarService _cars; private readonly IBrandService _brands;
    private readonly ICarModelService _models; private readonly IBranchService _branches;
    public FleetController(ICarService cars, IBrandService brands, ICarModelService models, IBranchService branches)
        => (_cars, _brands, _models, _branches) = (cars, brands, models, branches);

    public async Task<IActionResult> Index([FromQuery] CarFilterDto filter)
    {
        var model = new FleetViewModel { Filter = filter, Cars = await _cars.TGetFilteredCarsAsync(filter),
            Brands = await _brands.TGetWithActiveCarsAsync(), Models = await _models.TGetWithActiveCarsAsync(), Branches = await _branches.TGetListAsync() };
        return View(model);
    }
}
