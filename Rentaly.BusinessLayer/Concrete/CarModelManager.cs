using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarModelManager : ICarModelService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CarModel> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CarModel>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(CarModel entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(CarModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
