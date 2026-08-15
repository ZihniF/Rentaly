using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfCarModelDal : GenericRepository<CarModel>, ICarModelDal
    {
        private readonly RentalyContext _context;

        public EfCarModelDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<CarModel>> GetAllWithBrandAsync() =>
            _context.CarModels
                .AsNoTracking()
                .Include(x => x.Brand)
                .OrderBy(x => x.Brand.BrandName)
                .ThenBy(x => x.ModelName)
                .ToListAsync();

        public Task<List<CarModel>> GetWithActiveCarsAsync() =>
            _context.CarModels
                .AsNoTracking()
                .Include(x => x.Brand)
                .Where(x => x.Cars.Any(car => car.IsActive && car.IsAvailable))
                .OrderBy(x => x.Brand.BrandName)
                .ThenBy(x => x.ModelName)
                .ToListAsync();

        public Task<bool> ModelNameExistsAsync(int brandId, string modelName, int? excludedModelId = null)
        {
            var normalizedName = modelName.Trim().ToLower();
            return _context.CarModels.AsNoTracking().AnyAsync(x =>
                x.BrandId == brandId &&
                x.ModelName.ToLower() == normalizedName &&
                (!excludedModelId.HasValue || x.CarModelId != excludedModelId.Value));
        }
    }
}
