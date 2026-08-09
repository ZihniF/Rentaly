using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BranchManager : IBranchService
    {
        private readonly IBranchDal _branchDal;

        public BranchManager(IBranchDal branchDal)
        {
            _branchDal = branchDal;
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Branch> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Branch>> TGetListAsync()
        {
            return await _branchDal.GetListAsync();
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
