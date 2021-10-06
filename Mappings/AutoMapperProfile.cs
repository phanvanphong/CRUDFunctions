using AutoMapper;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotNet5.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category,CategoryViewModel >().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>()
            .ForMember(dest => 
                dest.ImageName,
                opt =>opt.MapFrom(src => src.Image)
            ).ReverseMap()
            ;
        }
    }
}
