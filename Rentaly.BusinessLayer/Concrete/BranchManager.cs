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

        public async Task TDeleteAsync(int id)
        {
            await _branchDal.ArchiveAsync(id);
        }

        public async Task<Branch> TGetByIdAsync(int id)
        {
            return await _branchDal.GetByIdAsync(id); 
        }

        public async Task<List<Branch>> TGetListAsync()
        {
            return await _branchDal.GetActiveListAsync();
        }

        public async Task TInsertAsync(Branch entity)
        {
            await _branchDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Branch entity)
        {
            await _branchDal.UpdateAsync(entity);
        }
        
    }
}
