using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarService:IGenericService<Car>
    {
        Task<List<Car>> TGetAllCarsWithCategoryAsync();
    }
}
