using AutoMapper;
using ECommerce.Domain.Entities.Product;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile() {

            CreateMap<ProductBrand, ProductBrandDTO>();
            CreateMap<ProductType, ProductTypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dst => dst.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
        }
    }
}
