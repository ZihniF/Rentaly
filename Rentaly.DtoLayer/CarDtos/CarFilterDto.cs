namespace Rentaly.DtoLayer.CarDtos;

public class CarFilterDto
{
    public int? BrandId { get; set; }
    public int? ModelId { get; set; }
    public int? BranchId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? PickupDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
