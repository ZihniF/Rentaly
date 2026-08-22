using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework;

public class EfHomePageSettingsDal : IHomePageSettingsDal
{
    private readonly RentalyContext _context;

    public EfHomePageSettingsDal(RentalyContext context) => _context = context;

    public async Task<HomePageSettings> GetAsync() =>
        await _context.HomePageSettings.AsNoTracking().SingleAsync(x => x.HomePageSettingsId == 1);

    public async Task UpdateAsync(HomePageSettings settings)
    {
        var current = await _context.HomePageSettings.SingleAsync(x => x.HomePageSettingsId == 1);
        _context.Entry(current).CurrentValues.SetValues(settings);
        current.HomePageSettingsId = 1;
        await _context.SaveChangesAsync();
    }
}
