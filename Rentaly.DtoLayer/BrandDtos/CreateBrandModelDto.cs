using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.BrandDtos;

public class CreateBrandModelDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Marka seçimi zorunludur.")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Model adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Model adı en fazla 100 karakter olabilir.")]
    public string ModelName { get; set; } = string.Empty;
}
