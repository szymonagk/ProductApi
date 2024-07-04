using AutoMapper;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi
{
    public class ProductApiMappingProfile : Profile
    {
        public ProductApiMappingProfile() 
        {
            CreateMap<Product, ProductWithUserFlagDTO>();
        }
    }
}
