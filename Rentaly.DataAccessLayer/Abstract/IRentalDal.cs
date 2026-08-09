using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IRentalDal : IGerenicDal<Rental>
    {
        Task<List<Rental>> GetRentalsWithDetailsAsync();

        Task<Rental?> GetRentalWithDetailsByIdAsync(int id);

        Task<bool> HasDateConflictAsync(
            int carId,
            DateTime pickupDate,
            DateTime returnDate,
            int? excludedRentalId = null);

        Task<bool> TryCreateRentalAsync(Rental rental);

        Task<bool> TryCreateBookingAsync(Customer customer, Rental rental);
    }
}
