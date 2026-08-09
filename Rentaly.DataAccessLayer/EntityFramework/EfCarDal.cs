using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Rentaly.DtoLayer.CarDtos;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfCarDal : GenericRepository<Car>, ICarDal
    {
        private readonly RentalyContext _rentalyContext;
        public EfCarDal(RentalyContext context) : base(context)
        {
            _rentalyContext = context;
        }

        public async Task<List<Car>> GetAllCarsWithCategoryAsync()
        {
            var values = await _rentalyContext.Cars
            .Include(x => x.Category).Include(x => x.Brand).Include(x => x.Model).Include(x => x.Branch)
            .ToListAsync();

            return values;
        }

        public async Task<List<Car>> GetFilteredCarsAsync(CarFilterDto filter, int? take = null)
        {
            var query = _rentalyContext.Cars.AsNoTracking()
                .Include(x => x.Category).Include(x => x.Brand)
                .Include(x => x.Model).Include(x => x.Branch)
                .Where(x => x.IsActive && x.IsAvailable);

            if (filter.BrandId.HasValue) query = query.Where(x => x.BrandId == filter.BrandId);
            if (filter.ModelId.HasValue) query = query.Where(x => x.ModelId == filter.ModelId);
            if (filter.BranchId.HasValue) query = query.Where(x => x.BranchId == filter.BranchId);
            if (filter.MinPrice.HasValue) query = query.Where(x => x.DailyPrice >= filter.MinPrice);
            if (filter.MaxPrice.HasValue) query = query.Where(x => x.DailyPrice <= filter.MaxPrice);
            if (filter.PickupDate.HasValue && filter.ReturnDate.HasValue)
            {
                var start = filter.PickupDate.Value;
                var end = filter.ReturnDate.Value;
                query = query.Where(car => !car.Rentals.Any(r =>
                    (r.Status == RentalStatus.Pending || r.Status == RentalStatus.Approved) &&
                    start < r.ReturnDate && end > r.PickupDate));
            }

            query = query.OrderBy(x => x.CarId);
            if (take.HasValue) query = query.Take(take.Value);
            return await query.ToListAsync();
        }

        public Task<Car?> GetCarWithDetailsAsync(int id) => _rentalyContext.Cars.AsNoTracking()
            .Include(x => x.Category).Include(x => x.Brand).Include(x => x.Model).Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.CarId == id);
    }
}
