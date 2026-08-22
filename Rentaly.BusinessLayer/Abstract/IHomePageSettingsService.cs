using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.BusinessLayer.Abstract;

public interface IHomePageSettingsService
{
    Task<HomePageSettingsDto> TGetAsync();
    Task TUpdateAsync(HomePageSettingsDto settings);
}
