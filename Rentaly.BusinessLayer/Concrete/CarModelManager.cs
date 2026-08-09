using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarModelManager : ICarModelService
    {
        private readonly ICarModelDal _dal;
        public CarModelManager(ICarModelDal dal) => _dal = dal;
        public Task TDeleteAsync(int id)
        {
            return _dal.DeleteAsync(id);
        }

        public Task<CarModel> TGetByIdAsync(int id)
        {
            return _dal.GetByIdAsync(id);
        }

        public Task<List<CarModel>> TGetListAsync()
        {
            return _dal.GetListAsync();
        }

        public Task<List<CarModel>> TGetAllWithBrandAsync()
        {
            return _dal.GetAllWithBrandAsync();
        }

        public Task<List<CarModel>> TGetWithActiveCarsAsync()
        {
            return _dal.GetWithActiveCarsAsync();
        }

        public Task TInsertAsync(CarModel entity)
        {
            return _dal.InsertAsync(entity);
        }

        public Task TUpdateAsync(CarModel entity)
        {
            return _dal.UpdateAsync(entity);
        }
    }
}
