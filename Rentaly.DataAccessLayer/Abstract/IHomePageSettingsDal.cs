using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract;

public interface IHomePageSettingsDal
{
    Task<HomePageSettings> GetAsync();
    Task UpdateAsync(HomePageSettings settings);
}
