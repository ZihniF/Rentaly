using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IBranchDal :IGerenicDal<Branch>
    {
        Task<List<Branch>> GetActiveListAsync();
        Task ArchiveAsync(int id);
    }
}
