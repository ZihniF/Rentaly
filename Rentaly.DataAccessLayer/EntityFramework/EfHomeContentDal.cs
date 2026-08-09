using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework;

public class EfHomeContentDal : GenericRepository<HomeContent>, IHomeContentDal
{
    private readonly RentalyContext _context;
    public EfHomeContentDal(RentalyContext context) : base(context) => _context = context;
    public Task<List<HomeContent>> GetActiveAsync() => _context.HomeContents.AsNoTracking()
        .Where(x => x.IsActive).OrderBy(x => x.Section).ThenBy(x => x.DisplayOrder).ToListAsync();
}
