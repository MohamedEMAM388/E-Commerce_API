using ECommerce.Presentation.Attrbuites;
using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{

    public class ProductsController : ApiBaseController
    {
        private readonly IProductServies _productServies;

        public ProductsController(IProductServies productServies)
        {
            _productServies = productServies;
        }

        // get all products

        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts(
          [FromQuery]  ProductQueryParams queryParams)
        {

            var products = await _productServies.GetAllProductAsync(queryParams);
            return Ok(products);

        }

        // get product by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {

            
            var result = await _productServies.GetProductAsync(id);
            return HandleResult<ProductDTO>(result);
        }

        // get brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrandDTO>>> GetAllBrands()
        {
            var brands = await _productServies.GetAllBrandsAsync();
            return Ok(brands);
        }

        // get types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ProductTypeDTO>>> GetAllTypes()
        {
            var types = await _productServies.GetAllTypesAsync();
            return Ok(types);

        }
    }
}
