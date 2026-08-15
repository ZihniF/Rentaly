using Rentaly.DtoLayer.CarDtos;

namespace Rentaly.DtoLayer.PageDtos;

public class FleetPageDto
{
    public CarFilterDto Filter { get; set; } = new();
    public List<CarCardDto> Cars { get; set; } = [];
    public List<BrandOptionDto> Brands { get; set; } = [];
    public List<CarModelOptionDto> Models { get; set; } = [];
    public List<BranchOptionDto> Branches { get; set; } = [];
}
