using AutoMapper;
using Rentaly.DtoLayer.CustomerDtos;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<Customer,CreateCustomerDto>().ReverseMap();
            CreateMap<Customer,ResultCustomerDto>().ReverseMap();
            CreateMap<Customer,GetCustomerByIdDto>().ReverseMap();
            CreateMap<Customer,UpdateCustomerDto>().ReverseMap();
            
            
        }
    }
}
