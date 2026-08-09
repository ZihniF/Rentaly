namespace Rentaly.DtoLayer.RentalDtos;

public class CreateBookingDto
{
    public int CarId { get; set; }
    public int PickupBranchId { get; set; }
    public int ReturnBranchId { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? IdentityNumber { get; set; }
}
