using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
            .Include(x => x.Category)
            .ToListAsync();

            return values;
        }
    }
}
