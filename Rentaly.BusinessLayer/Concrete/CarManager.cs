using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Rentaly.DtoLayer.CarDtos;
using FluentValidation;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarManager : ICarService
    {
        private readonly ICarDal _carDal;
        private readonly ICarModelDal _modelDal;
        private readonly IValidator<Car> _validator;

        public CarManager(ICarDal carDal, ICarModelDal modelDal, IValidator<Car> validator)
        {
            _carDal = carDal;
            _modelDal = modelDal;
            _validator = validator;
        }

        public async Task TDeleteAsync(int id)
        {
            await _carDal.DeleteAsync(id);
        }

        public async Task<List<Car>> TGetAllCarsWithCategoryAsync()
        {
            return await _carDal.GetAllCarsWithCategoryAsync();
        }

        public Task<List<Car>> TGetFilteredCarsAsync(CarFilterDto filter, int? take = null)
            => _carDal.GetFilteredCarsAsync(filter, take);

        public Task<Car?> TGetCarWithDetailsAsync(int id) => _carDal.GetCarWithDetailsAsync(id);

        public async Task<Car> TGetByIdAsync(int id)
        {
            return await _carDal.GetByIdAsync(id);
        }

        public async Task<List<Car>> TGetListAsync()
        {
            return await _carDal.GetListAsync();
        }

        public async Task TInsertAsync(Car entity)
        {
            await ValidateAsync(entity);
            await _carDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Car entity)
        {
            await ValidateAsync(entity);
            await _carDal.UpdateAsync(entity);
        }

        private async Task ValidateAsync(Car entity)
        {
            await _validator.ValidateAndThrowAsync(entity);
            var model = await _modelDal.GetByIdAsync(entity.ModelId);
            if (model.BrandId != entity.BrandId)
                throw new ValidationException("Seçilen model seçilen markaya ait değil.");
        }
    }
}
