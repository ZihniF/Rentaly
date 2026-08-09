using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract;

public interface IHomeContentService : IGenericService<HomeContent>
{
    Task<List<HomeContent>> TGetActiveAsync();
}
