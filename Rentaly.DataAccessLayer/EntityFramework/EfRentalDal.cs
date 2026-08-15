using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;
using System.Data;

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
        public async Task<bool> TryCreateRentalAsync(Rental rental)
        {
            await using var transaction =
                await _rentalyContext.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            var hasConflict =
                await _rentalyContext.Rentals.AnyAsync(x =>
                    x.CarId == rental.CarId &&
                    (x.Status == RentalStatus.Pending ||
                     x.Status == RentalStatus.Approved) &&
                    rental.PickupDate < x.ReturnDate &&
                    rental.ReturnDate > x.PickupDate);

            if (hasConflict)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await _rentalyContext.Rentals.AddAsync(rental);
            await _rentalyContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> TryCreateBookingAsync(Customer customer, Rental rental)
        {
            await using var transaction = await _rentalyContext.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

            var hasConflict = await _rentalyContext.Rentals.AnyAsync(x =>
                x.CarId == rental.CarId &&
                (x.Status == RentalStatus.Pending || x.Status == RentalStatus.Approved) &&
                rental.PickupDate < x.ReturnDate &&
                rental.ReturnDate > x.PickupDate);

            if (hasConflict)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await _rentalyContext.Customers.AddAsync(customer);
            await _rentalyContext.SaveChangesAsync();
            rental.CustomerId = customer.CustomerId;
            await _rentalyContext.Rentals.AddAsync(rental);
            await _rentalyContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }

        public async Task<bool> TryUpdateRentalAsync(Rental rental)
        {
            await using var transaction = await _rentalyContext.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

            var current = await _rentalyContext.Rentals
                .FirstOrDefaultAsync(x => x.RentalId == rental.RentalId);
            if (current is null)
                throw new KeyNotFoundException("Rezervasyon bulunamadı.");
            if (current.Status != RentalStatus.Pending)
                throw new InvalidOperationException("Yalnızca bekleyen rezervasyonlar düzenlenebilir.");

            var hasConflict = await _rentalyContext.Rentals.AnyAsync(x =>
                x.RentalId != rental.RentalId &&
                x.CarId == rental.CarId &&
                (x.Status == RentalStatus.Pending || x.Status == RentalStatus.Approved) &&
                rental.PickupDate < x.ReturnDate &&
                rental.ReturnDate > x.PickupDate);

            if (hasConflict)
            {
                await transaction.RollbackAsync();
                return false;
            }

            current.CarId = rental.CarId;
            current.CustomerId = rental.CustomerId;
            current.PickupBranchId = rental.PickupBranchId;
            current.ReturnBranchId = rental.ReturnBranchId;
            current.PickupDate = rental.PickupDate;
            current.ReturnDate = rental.ReturnDate;
            current.TotalPrice = rental.TotalPrice;

            await _rentalyContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
    }
}
