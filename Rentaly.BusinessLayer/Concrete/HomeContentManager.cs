using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete;

public class HomeContentManager : IHomeContentService
{
    private readonly IHomeContentDal _dal;
    public HomeContentManager(IHomeContentDal dal) => _dal = dal;
    public Task TInsertAsync(HomeContent entity) => _dal.InsertAsync(entity);
    public Task TDeleteAsync(int id) => _dal.DeleteAsync(id);
    public Task TUpdateAsync(HomeContent entity) => _dal.UpdateAsync(entity);
    public Task<List<HomeContent>> TGetListAsync() => _dal.GetListAsync();
    public Task<HomeContent> TGetByIdAsync(int id) => _dal.GetByIdAsync(id);
    public Task<List<HomeContent>> TGetActiveAsync() => _dal.GetActiveAsync();
}
