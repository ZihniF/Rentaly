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
    public class EfCustomerDal : GenericRepository<Customer>, ICustomerDal
    {
        private readonly RentalyContext _context;

        public EfCustomerDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Customer>> GetActiveListAsync() => _context.Customers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Surname)
            .ToListAsync();

        public async Task<Customer> GetActiveByIdAsync(int id) =>
            await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id && x.IsActive)
            ?? throw new KeyNotFoundException("Müşteri kaydı bulunamadı.");

        public async Task ArchiveAsync(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == id && x.IsActive)
                ?? throw new KeyNotFoundException("Müşteri kaydı bulunamadı.");

            customer.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
