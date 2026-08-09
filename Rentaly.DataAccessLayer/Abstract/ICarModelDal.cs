using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarModelDal : IGerenicDal<CarModel>
    {
        Task<List<CarModel>> GetAllWithBrandAsync();
        Task<List<CarModel>> GetWithActiveCarsAsync();
    }
}
