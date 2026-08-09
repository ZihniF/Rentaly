using System.ComponentModel.DataAnnotations;

namespace Rentaly.EntityLayer.Enums;

public enum HomeSectionType
{
    [Display(Name = "Process / İşleyiş")]
    Process = 1,

    [Display(Name = "Our Future / Geleceğimiz")]
    Future = 2,

    [Display(Name = "İstatistikler / About")]
    Statistic = 3,

    [Display(Name = "Ödüller")]
    Award = 4,

    [Display(Name = "Testimonial / Müşteri Yorumları")]
    Testimonial = 5,

    [Display(Name = "Sık Sorulan Sorular (FAQ)")]
    Faq = 6
}
