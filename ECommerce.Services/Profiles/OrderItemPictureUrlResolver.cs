using AutoMapper;
using ECommerce.Domain.Entities.OrderModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class OrderItemPictureUrlResolver : IValueResolver<ItemsOrder, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(ItemsOrder source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            // check if the src picture url is null or empty
            if(string.IsNullOrEmpty(source.product.PictureUrl))
                return string.Empty;

            // check if the picture url is already an absolute url (start with http | https)
            if(source.product.PictureUrl.StartsWith("http") 
               || source.product.PictureUrl.StartsWith("https"))
                return source.product.PictureUrl;

            var baseurl = _configuration.GetSection("URLS")["BaseUrl"];
            if(string.IsNullOrEmpty(baseurl))
                      return string.Empty;
            return $"{baseurl}{source.product.PictureUrl}";
        }
    }
}
