using Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IBasketServies
    {
        // create or update basket
        Task<CustomerBasketDTO> CreateOrUpdateBasketAsync(CustomerBasketDTO BasketDto);

        // get basket by id
        Task<CustomerBasketDTO> GetBasketAsync(string basketId);
        // delete basket by id
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
