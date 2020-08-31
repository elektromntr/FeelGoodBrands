using AutoMapper;
using Data.Models;
using Logic.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Maps
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, EditProduct>();
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand.Name));
        }
    }
}