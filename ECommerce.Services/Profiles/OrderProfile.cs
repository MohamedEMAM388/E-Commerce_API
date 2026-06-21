using AutoMapper;
using ECommerce.Domain.Entities.OrderModule;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class OrderProfile : Profile
    {

        public OrderProfile() {

            CreateMap<AddressDTO, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDTO>()
                    .ForMember(dst => dst.DeliveryMethod,
                     opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                    .ForMember(dst => dst.OrderStatus,
                     opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ItemsOrder, OrderItemDTO>()
                     .ForMember(dst => dst.ProductName,
                      opt => opt.MapFrom(src => src.product.ProductName))
                     .ForMember(dst => dst.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
        }
    }
}
