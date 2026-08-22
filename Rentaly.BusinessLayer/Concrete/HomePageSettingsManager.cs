using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DtoLayer.HomeDtos;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete;

public class HomePageSettingsManager : IHomePageSettingsService
{
    private readonly IHomePageSettingsDal _dal;

    public HomePageSettingsManager(IHomePageSettingsDal dal) => _dal = dal;

    public async Task<HomePageSettingsDto> TGetAsync() => ToDto(await _dal.GetAsync());

    public Task TUpdateAsync(HomePageSettingsDto settings) => _dal.UpdateAsync(ToEntity(settings));

    private static HomePageSettingsDto ToDto(HomePageSettings x) => new()
    {
        HomePageSettingsId = x.HomePageSettingsId,
        HeroEyebrow = x.HeroEyebrow, HeroTitle = x.HeroTitle, HeroAccentText = x.HeroAccentText,
        HeroSecondLine = x.HeroSecondLine, HeroDescription = x.HeroDescription,
        HeroBackgroundUrl = x.HeroBackgroundUrl, ProcessEyebrow = x.ProcessEyebrow,
        ProcessTitle = x.ProcessTitle, CarsEyebrow = x.CarsEyebrow, CarsTitle = x.CarsTitle,
        FutureEyebrow = x.FutureEyebrow, FutureBackgroundUrl = x.FutureBackgroundUrl,
        StatisticsEyebrow = x.StatisticsEyebrow, StatisticsTitle = x.StatisticsTitle,
        BrandsEyebrow = x.BrandsEyebrow, BrandsTitle = x.BrandsTitle,
        AwardsEyebrow = x.AwardsEyebrow, AwardsTitle = x.AwardsTitle,
        TestimonialsEyebrow = x.TestimonialsEyebrow, TestimonialsTitle = x.TestimonialsTitle,
        FaqEyebrow = x.FaqEyebrow, FaqTitle = x.FaqTitle, FaqDescription = x.FaqDescription,
        FooterDescription = x.FooterDescription, ContactPhone = x.ContactPhone,
        ContactEmail = x.ContactEmail, WorkingHours = x.WorkingHours,
        RoadsideAssistance = x.RoadsideAssistance
    };

    private static HomePageSettings ToEntity(HomePageSettingsDto x) => new()
    {
        HomePageSettingsId = 1,
        HeroEyebrow = x.HeroEyebrow.Trim(), HeroTitle = x.HeroTitle.Trim(),
        HeroAccentText = x.HeroAccentText.Trim(), HeroSecondLine = x.HeroSecondLine.Trim(),
        HeroDescription = x.HeroDescription.Trim(), HeroBackgroundUrl = x.HeroBackgroundUrl.Trim(),
        ProcessEyebrow = x.ProcessEyebrow.Trim(), ProcessTitle = x.ProcessTitle.Trim(),
        CarsEyebrow = x.CarsEyebrow.Trim(), CarsTitle = x.CarsTitle.Trim(),
        FutureEyebrow = x.FutureEyebrow.Trim(), FutureBackgroundUrl = x.FutureBackgroundUrl.Trim(),
        StatisticsEyebrow = x.StatisticsEyebrow.Trim(), StatisticsTitle = x.StatisticsTitle.Trim(),
        BrandsEyebrow = x.BrandsEyebrow.Trim(), BrandsTitle = x.BrandsTitle.Trim(),
        AwardsEyebrow = x.AwardsEyebrow.Trim(), AwardsTitle = x.AwardsTitle.Trim(),
        TestimonialsEyebrow = x.TestimonialsEyebrow.Trim(), TestimonialsTitle = x.TestimonialsTitle.Trim(),
        FaqEyebrow = x.FaqEyebrow.Trim(), FaqTitle = x.FaqTitle.Trim(),
        FaqDescription = x.FaqDescription.Trim(), FooterDescription = x.FooterDescription.Trim(),
        ContactPhone = x.ContactPhone.Trim(), ContactEmail = x.ContactEmail.Trim(),
        WorkingHours = x.WorkingHours.Trim(), RoadsideAssistance = x.RoadsideAssistance.Trim()
    };
}
