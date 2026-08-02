using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IGerenicDal<T>
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<List<T>> GetListAsync();
        Task<T> GetByIdAsync(int id);
    }
}
