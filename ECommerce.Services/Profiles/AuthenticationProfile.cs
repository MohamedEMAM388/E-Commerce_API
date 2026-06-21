using AutoMapper;
using ECommerce.Domain.Entities.IdentityModule;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class AuthenticationProfile : Profile
    {

        public AuthenticationProfile() {

            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
