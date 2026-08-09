using AutoMapper;
using Rentaly.DtoLayer.CustomerDtos;
using Rentaly.EntityLayer.Entities;
using Rentaly.DtoLayer.RentalDtos;
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

            CreateMap < CreateRentalDto, Rental > ()
                .ForMember(x => x.RentalId, opt => opt.Ignore())
                .ForMember(x => x.TotalPrice, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Car, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt => opt.Ignore())
                .ForMember(x => x.PickupBranch, opt => opt.Ignore())
                .ForMember(x => x.ReturnBranch, opt => opt.Ignore());

            CreateMap<Rental, GetRentalByIdDto>();

            CreateMap<Rental, ResultRentalDto>()
                .ForMember(x => x.PlateNumber,
                    opt => opt.MapFrom(x => x.Car.PlateNumber))
                .ForMember(x => x.BrandName,
                    opt => opt.MapFrom(x => x.Car.Brand.BrandName))
                .ForMember(x => x.ModelName,
                    opt => opt.MapFrom(x => x.Car.Model.ModelName))
                .ForMember(x => x.CustomerFullName,
                    opt => opt.MapFrom(x => x.Customer.Name + " " + x.Customer.Surname))
                .ForMember(x => x.CustomerEmail,
                    opt => opt.MapFrom(x => x.Customer.Email))
                .ForMember(x => x.PickupBranchName,
                    opt => opt.MapFrom(x => x.PickupBranch.BranchName))
                .ForMember(x => x.ReturnBranchName,
                    opt => opt.MapFrom(x => x.ReturnBranch.BranchName));
        }
    }
}
