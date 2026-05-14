using ECommerce.Domain.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contarct
{
    public interface IBasketRepository
    {

        // create or update basket
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan TimeToLive = default);

        // Get basket by id
        Task<CustomerBasket?> GetBasketAsync(string basketId);

        // Delete basket by id
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
