using ECommerce.Domain.Entities.Product;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications.ProductSpecifications
{
    internal class ProductWithTypeAndBrandSpecification : BaseSpecification<Product, int>
    {
        public ProductWithTypeAndBrandSpecification(ProductQueryParams queryParams) : base
            (ProductSpecificationHelper.GetCriteria(queryParams))
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            // switch case for sorting
            switch (queryParams.Sort) {

                case ProductSorting.PriceAsc:
                    AddOrderBy(p => p.Price); 
                    break;
                case ProductSorting.PriceDesc:
                     AddOrderByDescinding(p => p.Price);
                     break;
                case ProductSorting.NameAsc:
                     AddOrderBy(p => p.Name);
                     break;
                case ProductSorting.NameDesc:
                     AddOrderByDescinding(p => p.Name);
                     break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplayPagination(queryParams.PageSize, queryParams.PageIndex);
            

            
        }


        // spec for get product by id
        public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id) 
        {

            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
