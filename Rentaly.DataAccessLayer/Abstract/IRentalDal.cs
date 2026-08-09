using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
