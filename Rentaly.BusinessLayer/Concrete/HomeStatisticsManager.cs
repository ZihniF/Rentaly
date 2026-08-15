using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.BusinessLayer.Concrete;

public class HomeStatisticsManager : IHomeStatisticsService
{
    private readonly IHomeStatisticsDal _statisticsDal;

    public HomeStatisticsManager(IHomeStatisticsDal statisticsDal) => _statisticsDal = statisticsDal;

    public Task<HomeStatisticsDto> TGetAsync() => _statisticsDal.GetAsync();
}
