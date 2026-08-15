using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IBrandDal : IGerenicDal<Brand>
    {
        Task<List<Brand>> GetWithActiveCarsAsync();
        Task<List<Brand>> GetAllWithModelsAsync();
    }
}
