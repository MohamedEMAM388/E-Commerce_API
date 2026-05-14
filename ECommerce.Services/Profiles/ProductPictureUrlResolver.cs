using AutoMapper;
using ECommerce.Domain.Entities.Product;
using Microsoft.Extensions.Configuration;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Profiles
{
    internal class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            // check if the src.picurl is not null or empty
            if(string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;

            // check if the src.picurl already contains "http" or "https"
            if(source.PictureUrl.StartsWith("http") || source.PictureUrl.StartsWith("https"))
                return source.PictureUrl;
            var Baseurl = _configuration.GetSection("URLS")["BaseUrl"];
            var PictureUrl = $"{Baseurl}{source.PictureUrl}";

            return PictureUrl;


        }
    }
}
