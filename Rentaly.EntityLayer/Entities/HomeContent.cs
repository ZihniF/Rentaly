using System.ComponentModel.DataAnnotations;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.EntityLayer.Entities;

public class HomeContent
{
    public int HomeContentId { get; set; }

    [Required(ErrorMessage = "Bölüm seçimi zorunludur.")]
    public HomeSectionType Section { get; set; }

    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(180, ErrorMessage = "Başlık en fazla 180 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(180, ErrorMessage = "Alt başlık en fazla 180 karakter olabilir.")]
    public string? Subtitle { get; set; }

    [StringLength(1500, ErrorMessage = "Açıklama en fazla 1500 karakter olabilir.")]
    public string? Description { get; set; }

    [StringLength(500, ErrorMessage = "Görsel adresi en fazla 500 karakter olabilir.")]
    public string? ImageUrl { get; set; }

    [StringLength(100, ErrorMessage = "İkon bilgisi en fazla 100 karakter olabilir.")]
    public string? Icon { get; set; }

    [Range(0, 999, ErrorMessage = "Sıralama 0 ile 999 arasında olmalıdır.")]
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
