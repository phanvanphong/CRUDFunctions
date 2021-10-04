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
            CreateMap<Category,CategoryViewModel >();
            CreateMap<CategoryViewModel,Category >();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
