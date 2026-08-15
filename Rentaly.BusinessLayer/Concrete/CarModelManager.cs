using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

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

        public async Task TInsertAsync(CarModel entity)
        {
            await ValidateAsync(entity);
            await _dal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(CarModel entity)
        {
            await ValidateAsync(entity);
            await _dal.UpdateAsync(entity);
        }

        private async Task ValidateAsync(CarModel entity)
        {
            entity.ModelName = entity.ModelName.Trim();
            if (entity.BrandId <= 0)
                throw new ValidationException("Marka seçimi zorunludur.");
            if (string.IsNullOrWhiteSpace(entity.ModelName))
                throw new ValidationException("Model adı zorunludur.");
            if (await _dal.ModelNameExistsAsync(entity.BrandId, entity.ModelName,
                    entity.CarModelId > 0 ? entity.CarModelId : null))
                throw new ValidationException("Bu model ilgili marka altında zaten kayıtlı.");
        }
    }
}
