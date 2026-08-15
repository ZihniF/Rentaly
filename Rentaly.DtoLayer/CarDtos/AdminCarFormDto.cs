using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.CarDtos;

public class AdminCarFormDto
{
    public int CarId { get; set; }

    [Required(ErrorMessage = "Plaka boş geçilemez.")]
    [StringLength(10, ErrorMessage = "Plaka en fazla 10 karakter olabilir.")]
    public string PlateNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şasi numarası boş geçilemez.")]
    [StringLength(17, MinimumLength = 17, ErrorMessage = "Şasi numarası 17 karakter olmalıdır.")]
    public string VIN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Marka seçimi yapılmalıdır.")]
    public int BrandId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Model seçimi yapılmalıdır.")]
    public int ModelId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kategori seçimi yapılmalıdır.")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Şube seçimi yapılmalıdır.")]
    public int BranchId { get; set; }

    [Range(1990, 2100, ErrorMessage = "Araç yılı geçerli bir değer olmalıdır.")]
    public int Year { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Kilometre negatif olamaz.")]
    public int Kilometer { get; set; }

    [Range(0.01, 999999999.0, ErrorMessage = "Günlük fiyat 0'dan büyük olmalıdır.")]
    public decimal DailyPrice { get; set; }

    [Range(0.0, 999999999.0, ErrorMessage = "Depozito negatif olamaz.")]
    public decimal DepositAmount { get; set; }

    public bool IsAvailable { get; set; } = true;
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Araç görseli boş olamaz.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Range(1, 12, ErrorMessage = "Koltuk sayısı 1 ile 12 arasında olmalıdır.")]
    public int SeatCount { get; set; } = 5;

    [Range(0, int.MaxValue, ErrorMessage = "Bagaj sayısı negatif olamaz.")]
    public int LuggageCount { get; set; }

    [Required(ErrorMessage = "Yakıt tipi boş geçilemez.")]
    public string FuelType { get; set; } = string.Empty;
}
