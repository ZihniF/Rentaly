using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICategoryDal:IGerenicDal<Category>
    {
        Task<List<Category>> GetActiveListAsync();
        Task ArchiveAsync(int id);
    }
}
