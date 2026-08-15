namespace Rentaly.DtoLayer.RentalDtos;

public class UpdateRentalDto
{
    public int RentalId { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public int PickupBranchId { get; set; }
    public int ReturnBranchId { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
