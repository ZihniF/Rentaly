using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfBrandDal : GenericRepository<Brand>, IBrandDal
    {
        private readonly RentalyContext _context;

        public EfBrandDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Brand>> GetWithActiveCarsAsync() =>
            _context.Brands
                .AsNoTracking()
                .Where(x => x.Cars.Any(car => car.IsActive && car.IsAvailable))
                .OrderBy(x => x.BrandName)
                .ToListAsync();

        public Task<List<Brand>> GetAllWithModelsAsync() =>
            _context.Brands
                .AsNoTracking()
                .Include(x => x.CarModels.OrderBy(model => model.ModelName))
                    .ThenInclude(model => model.Cars)
                .OrderBy(x => x.BrandName)
                .ToListAsync();
    }
}
