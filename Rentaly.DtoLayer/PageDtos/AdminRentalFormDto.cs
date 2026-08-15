using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.PageDtos;

public class AdminRentalFormDto
{
    public int RentalId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Araç seçilmelidir.")]
    public int CarId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Müşteri seçilmelidir.")]
    public int CustomerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Teslim alma şubesi seçilmelidir.")]
    public int PickupBranchId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "İade şubesi seçilmelidir.")]
    public int ReturnBranchId { get; set; }

    [Required]
    public DateTime PickupDate { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    public List<CarCardDto> Cars { get; set; } = [];
    public List<CustomerOptionDto> Customers { get; set; } = [];
    public List<BranchOptionDto> Branches { get; set; } = [];
}

public class CustomerOptionDto
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
