using System.ComponentModel.DataAnnotations;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.Tests;

public class DtoValidationTests
{
    [Fact]
    public void AdminCarFormDto_RequiresValidVinAndPositiveSelections()
    {
        var dto = new AdminCarFormDto
        {
            PlateNumber = "34TEST34",
            VIN = "SHORT",
            Year = DateTime.Now.Year,
            DailyPrice = 1_000,
            ImageUrl = "/images/test.png",
            SeatCount = 5,
            FuelType = "Dizel"
        };

        var errors = Validate(dto);

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.VIN)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.BrandId)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.ModelId)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.CategoryId)));
        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.BranchId)));
    }

    [Fact]
    public void HomePageSettingsDto_RejectsInvalidContactEmail()
    {
        var dto = CreateValidSettings();
        dto.ContactEmail = "gecersiz-adres";

        var errors = Validate(dto);

        Assert.Contains(errors, x => x.MemberNames.Contains(nameof(dto.ContactEmail)));
    }

    [Fact]
    public void HomePageSettingsDto_AcceptsCompleteSettings()
    {
        Assert.Empty(Validate(CreateValidSettings()));
    }

    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, true);
        return results;
    }

    private static HomePageSettingsDto CreateValidSettings() => new()
    {
        HeroEyebrow = "Rentaly",
        HeroTitle = "Yolculuğunuzu",
        HeroAccentText = "özgürce",
        HeroSecondLine = "planlayın",
        HeroDescription = "Size uygun aracı bulun.",
        HeroBackgroundUrl = "/images/hero.jpg",
        ProcessEyebrow = "İşleyiş",
        ProcessTitle = "Üç kolay adım",
        CarsEyebrow = "Araçlar",
        CarsTitle = "Filomuzu keşfedin",
        FutureEyebrow = "Geleceğimiz",
        FutureBackgroundUrl = "/images/future.jpg",
        StatisticsEyebrow = "Hakkımızda",
        StatisticsTitle = "Rakamlarla Rentaly",
        BrandsEyebrow = "Markalar",
        BrandsTitle = "Tercih edilen markalar",
        AwardsEyebrow = "Ödüller",
        AwardsTitle = "Başarılarımız",
        TestimonialsEyebrow = "Yorumlar",
        TestimonialsTitle = "Misafirlerimiz ne diyor?",
        FaqEyebrow = "SSS",
        FaqTitle = "Merak ettikleriniz",
        FaqDescription = "Sık sorulan sorular.",
        FooterDescription = "Güvenli araç kiralama.",
        ContactPhone = "+90 212 000 00 00",
        ContactEmail = "info@rentaly.com",
        WorkingHours = "Her gün 09.00-22.00",
        RoadsideAssistance = "7/24 yol yardım"
    };
}
