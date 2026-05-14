using ECommerce.Domain.Entities.Product;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications.ProductSpecifications
{
    internal class ProductWithCountSpecification : BaseSpecification<Product , int>
    {
        public ProductWithCountSpecification(ProductQueryParams queryParams) : base
            (ProductSpecificationHelper.GetCriteria(queryParams))
        { 
        
        }
    }
}
