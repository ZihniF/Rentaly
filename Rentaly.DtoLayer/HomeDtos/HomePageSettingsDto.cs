using System.ComponentModel.DataAnnotations;

namespace Rentaly.DtoLayer.HomeDtos;

public class HomePageSettingsDto
{
    public int HomePageSettingsId { get; set; } = 1;

    [Required, StringLength(160)] public string HeroEyebrow { get; set; } = string.Empty;
    [Required, StringLength(200)] public string HeroTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string HeroAccentText { get; set; } = string.Empty;
    [Required, StringLength(200)] public string HeroSecondLine { get; set; } = string.Empty;
    [Required, StringLength(500)] public string HeroDescription { get; set; } = string.Empty;
    [Required, StringLength(500)] public string HeroBackgroundUrl { get; set; } = string.Empty;

    [Required, StringLength(120)] public string ProcessEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string ProcessTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string CarsEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string CarsTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string FutureEyebrow { get; set; } = string.Empty;
    [Required, StringLength(500)] public string FutureBackgroundUrl { get; set; } = string.Empty;
    [Required, StringLength(120)] public string StatisticsEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string StatisticsTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string BrandsEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string BrandsTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string AwardsEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string AwardsTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string TestimonialsEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string TestimonialsTitle { get; set; } = string.Empty;
    [Required, StringLength(120)] public string FaqEyebrow { get; set; } = string.Empty;
    [Required, StringLength(180)] public string FaqTitle { get; set; } = string.Empty;
    [Required, StringLength(400)] public string FaqDescription { get; set; } = string.Empty;

    [Required, StringLength(500)] public string FooterDescription { get; set; } = string.Empty;
    [Required, StringLength(80)] public string ContactPhone { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(160)] public string ContactEmail { get; set; } = string.Empty;
    [Required, StringLength(160)] public string WorkingHours { get; set; } = string.Empty;
    [Required, StringLength(160)] public string RoadsideAssistance { get; set; } = string.Empty;
}
