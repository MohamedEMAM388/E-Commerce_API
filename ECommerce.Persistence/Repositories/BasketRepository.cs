using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan TimeToLive)
        {

            var jsonbasket = JsonSerializer.Serialize(basket);
            await _database.StringSetAsync(basket.Id , jsonbasket ,(TimeToLive == default) ? TimeSpan.FromDays(7) : TimeToLive);


            return await GetBasketAsync(basket.Id);
        }

        public Task<bool> DeleteBasketAsync(string basketId) => _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var jsonBasket = await _database.StringGetAsync(basketId);
            // deserialize the json to customer basket object

            if (jsonBasket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(jsonBasket!);
        }
    }
}
