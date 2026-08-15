using Rentaly.DtoLayer.HomeDtos;

namespace Rentaly.DataAccessLayer.Abstract;

public interface IHomeStatisticsDal
{
    Task<HomeStatisticsDto> GetAsync();
}
