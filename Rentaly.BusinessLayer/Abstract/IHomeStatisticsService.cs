using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.BusinessLayer.Abstract;

public interface IHomeStatisticsService
{
    Task<HomeStatisticsDto> TGetAsync();
}
