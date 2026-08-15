using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IBrandService:IGenericService<Brand>
    {
        Task<List<Brand>> TGetWithActiveCarsAsync();
        Task<List<Brand>> TGetAllWithModelsAsync();
    }
}
