using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BranchManager : IBranchService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Branch> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Branch>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(Branch entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Branch entity)
        {
            throw new NotImplementedException();
        }
    }
}
