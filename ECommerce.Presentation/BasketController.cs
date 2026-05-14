using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServies _basketServies;

        public BasketController(IBasketServies basketServies)
        {
            _basketServies = basketServies;
        }
        // create or update basket

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdateBasket(
            CustomerBasketDTO customerBasketDTO){ 
        
            var basket = await _basketServies.CreateOrUpdateBasketAsync(customerBasketDTO);
            return Ok(basket);

        }

        // get basket by id
        [HttpGet] // from query string
        public async Task<ActionResult<CustomerBasketDTO>> GetBasket(string id) { 

            var basket = await _basketServies.GetBasketAsync(id);
            return Ok(basket);
        }

        // delete basket by id
        [HttpDelete("{id}")] //  from route
        public async Task<ActionResult<bool>> DeleteBasket(string id) { 
        
            var isDeleted = await _basketServies.DeleteBasketAsync(id);
            return Ok(isDeleted);
        }
    }
}
