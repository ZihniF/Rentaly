using Rentaly.DtoLayer.CustomerDtos;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> TGetListAsync();
        Task<GetCustomerByIdDto> TGetByIdAsync(int id);
        Task<int> TInsertAsync (CreateCustomerDto dto);
        Task TUpdateAsync (UpdateCustomerDto dto);
        Task TDeleteAsync (int id);
    }
}
