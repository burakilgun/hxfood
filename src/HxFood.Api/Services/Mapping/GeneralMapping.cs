using AutoMapper;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Models.Requests.Product;

namespace HxFood.Api.Services.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Product, ProductAddRequest>().ReverseMap();
            CreateMap<Product, ProductUpdateRequest>().ReverseMap();
            CreateMap<Category, CategoryAddRequest>().ReverseMap();
            CreateMap<Category, CategoryUpdateRequest>().ReverseMap();
        }
    }
}