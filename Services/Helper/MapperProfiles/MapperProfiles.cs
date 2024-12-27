using AutoMapper;
using BuildLease.Data.DTOs.Product;
using Data.DTOs.Brand;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            // Product
            CreateMap<Product, ProductResponseDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name));

            CreateMap<ProductRequestDTO, Product>();
            CreateMap<ProductUpdateRequestDTO, Product>()
             .ForMember(dest => dest.DiscountStartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DiscountStartDate)))
             .ForMember(dest => dest.DiscountEndDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DiscountEndDate)))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Brand
            CreateMap<Brand, BrandResponseDTO>();
            CreateMap<BrandRequestDTO, Brand>();
        }
    }
}
