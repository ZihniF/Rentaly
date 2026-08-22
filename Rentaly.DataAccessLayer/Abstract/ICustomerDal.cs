using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICustomerDal : IGerenicDal<Customer>
    {
        Task<List<Customer>> GetActiveListAsync();
        Task<Customer> GetActiveByIdAsync(int id);
        Task ArchiveAsync(int id);
    }
}
