using Rentaly.DtoLayer.CarDtos;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Models;

public class FleetViewModel
{
    public CarFilterDto Filter { get; set; } = new();
    public List<Car> Cars { get; set; } = [];
    public List<Brand> Brands { get; set; } = [];
    public List<CarModel> Models { get; set; } = [];
    public List<Branch> Branches { get; set; } = [];
}
