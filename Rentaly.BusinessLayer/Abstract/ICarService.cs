using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Rentaly.DtoLayer.CarDtos;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarService:IGenericService<Car>
    {
        Task<List<Car>> TGetAllCarsWithCategoryAsync();
        Task<List<Car>> TGetFilteredCarsAsync(CarFilterDto filter, int? take = null);
        Task<Car?> TGetCarWithDetailsAsync(int id);
    }
}
