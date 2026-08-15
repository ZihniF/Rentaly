using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.PageDtos;

public class BookingPageDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Araç seçiniz.")]
    public int CarId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Teslim alma şubesi seçiniz.")]
    public int PickupBranchId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "İade şubesi seçiniz.")]
    public int ReturnBranchId { get; set; }

    [Required]
    public DateTime PickupDate { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Surname { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    public string Phone { get; set; } = string.Empty;

    [RegularExpression("^$|^[0-9]{11}$", ErrorMessage = "T.C. kimlik numarası 11 haneli olmalıdır.")]
    public string? IdentityNumber { get; set; }

    public List<CarCardDto> Cars { get; set; } = [];
    public List<BranchOptionDto> Branches { get; set; } = [];
}
