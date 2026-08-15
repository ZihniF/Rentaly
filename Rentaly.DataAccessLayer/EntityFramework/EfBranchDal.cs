using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfBranchDal:GenericRepository<Branch>, IBranchDal
    {
        private readonly RentalyContext _context;

        public EfBranchDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Branch>> GetActiveListAsync() => _context.Branches
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.BranchName)
            .ToListAsync();

        public async Task ArchiveAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var branch = await _context.Branches.FirstOrDefaultAsync(x => x.BranchId == id && x.IsActive)
                ?? throw new KeyNotFoundException("Şube kaydı bulunamadı.");

            branch.IsActive = false;
            var branchCars = await _context.Cars.Where(x => x.BranchId == id).ToListAsync();
            foreach (var car in branchCars)
            {
                car.IsActive = false;
                car.IsAvailable = false;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}
