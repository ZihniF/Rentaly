using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract;

public interface IHomeContentDal : IGerenicDal<HomeContent>
{
    Task<List<HomeContent>> GetActiveAsync();
}
