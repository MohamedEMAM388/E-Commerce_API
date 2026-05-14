using ECommerce.Domain.Entities.Product;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications.ProductSpecifications
{
    public static class ProductSpecificationHelper
    {
        public static Expression<Func<Product, bool>> GetCriteria(ProductQueryParams queryParams) {

           return p => (!queryParams.brandid.HasValue || p.ProductBrandId == queryParams.brandid.Value) &&
            (!queryParams.typeid.HasValue || p.ProductTypeId == queryParams.typeid.Value) &&
            (string.IsNullOrEmpty(queryParams.search) || p.Name.ToLower().Contains(queryParams.search.ToLower()));
        
        }
    }
}
