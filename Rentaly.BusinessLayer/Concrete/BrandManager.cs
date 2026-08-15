using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _brandDal.DeleteAsync(id);
        }

        public async Task<Brand> TGetByIdAsync(int id)
        {
            return await _brandDal.GetByIdAsync(id);
        }

        public async Task<List<Brand>> TGetListAsync()
        {
            return await _brandDal.GetListAsync();
        }

        public Task<List<Brand>> TGetWithActiveCarsAsync()
        {
            return _brandDal.GetWithActiveCarsAsync();
        }

        public Task<List<Brand>> TGetAllWithModelsAsync()
        {
            return _brandDal.GetAllWithModelsAsync();
        }


        public async Task TInsertAsync(Brand entity)
        {
            Validate(entity);
            await _brandDal.InsertAsync(entity);
        }


        public async Task TUpdateAsync(Brand entity)
        {
            Validate(entity);
            await _brandDal.UpdateAsync(entity);
        }

        private static void Validate(Brand entity)
        {
            var result = new BrandValidator().Validate(entity);
            if (!result.IsValid)
                throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
