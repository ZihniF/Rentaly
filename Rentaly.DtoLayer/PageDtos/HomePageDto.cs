namespace Rentaly.DtoLayer.PageDtos;

public class HomePageDto
{
    public List<HomeContentItemDto> Processes { get; set; } = [];
    public List<HomeContentItemDto> Futures { get; set; } = [];
    public List<HomeContentItemDto> Statistics { get; set; } = [];
    public List<HomeContentItemDto> Awards { get; set; } = [];
    public List<HomeContentItemDto> Testimonials { get; set; } = [];
    public List<HomeContentItemDto> Faqs { get; set; } = [];
    public List<CarCardDto> Cars { get; set; } = [];
    public List<BrandOptionDto> Brands { get; set; } = [];
    public List<CarModelOptionDto> Models { get; set; } = [];
    public List<BranchOptionDto> Branches { get; set; } = [];
    public int? SelectedBranchId { get; set; }
    public DateTime? PickupDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool AvailabilitySearchApplied { get; set; }
}
