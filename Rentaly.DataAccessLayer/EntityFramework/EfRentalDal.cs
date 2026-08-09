using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfRentalDal : GenericRepository<Rental>, IRentalDal
    {
        private readonly RentalyContext _rentalyContext;

        public EfRentalDal(RentalyContext context) : base(context)
        {
            _rentalyContext = context;
        }

        public async Task<List<Rental>> GetRentalsWithDetailsAsync()
        {
            return await _rentalyContext.Rentals
                .AsNoTracking()
                .Include(x => x.Car)
                    .ThenInclude(x => x.Brand)
                .Include(x => x.Car)
                    .ThenInclude(x => x.Model)
                .Include(x => x.Customer)
                .Include(x => x.PickupBranch)
                .Include(x => x.ReturnBranch)
                .OrderByDescending(x => x.RentalId)
                .ToListAsync();
        }

        public async Task<Rental?> GetRentalWithDetailsByIdAsync(int id)
        {
            return await _rentalyContext.Rentals
                .AsNoTracking()
                .Include(x => x.Car)
                    .ThenInclude(x => x.Brand)
                .Include(x => x.Car)
                    .ThenInclude(x => x.Model)
                .Include(x => x.Customer)
                .Include(x => x.PickupBranch)
                .Include(x => x.ReturnBranch)
                .FirstOrDefaultAsync(x => x.RentalId == id);
        }

        public async Task<bool> HasDateConflictAsync(
            int carId,
            DateTime pickupDate,
            DateTime returnDate,
            int? excludedRentalId = null)
        {
            return await _rentalyContext.Rentals.AnyAsync(x =>
                x.CarId == carId &&
                (!excludedRentalId.HasValue ||
                 x.RentalId != excludedRentalId.Value) &&
                (x.Status == RentalStatus.Pending ||
                 x.Status == RentalStatus.Approved) &&
                pickupDate < x.ReturnDate &&
                returnDate > x.PickupDate);
        }
    }
}