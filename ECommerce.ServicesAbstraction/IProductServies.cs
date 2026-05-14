using Shared;
using Shared.CommonResponses;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IProductServies
    {
        // get all products
        Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams);

        // get product by id

        Task<Result<ProductDTO>> GetProductAsync(int id);

        // get brands 
        Task<IEnumerable<ProductBrandDTO>> GetAllBrandsAsync();

        // get types
        Task<IEnumerable<ProductTypeDTO>> GetAllTypesAsync();
    }
}
