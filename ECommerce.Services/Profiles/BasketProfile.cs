using AutoMapper;
using ECommerce.Domain.Entities.BasketModule;
using Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile() { 
        
            CreateMap<CustomerBasket , CustomerBasketDTO>().ReverseMap();   
            CreateMap<BasketItem , BasketItemDTO>().ReverseMap();
        }
    }
}
