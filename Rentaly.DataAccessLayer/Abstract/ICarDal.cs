using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Rentaly.DtoLayer.CarDtos;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarDal : IGerenicDal<Car>
    {
        Task<List<Car>> GetAllCarsWithCategoryAsync();
        Task<List<Car>> GetFilteredCarsAsync(CarFilterDto filter, int? take = null);
        Task<Car?> GetCarWithDetailsAsync(int id);
    }
}
