using ECommerce.Domain.Contarct;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database; // to interact with the redis database
        public CacheRepository(IConnectionMultiplexer connection) // to get the connection to the redis
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetDataAsync(string CacheKey)
        {
           var CacheValue = await _database.StringGetAsync(CacheKey);

            return CacheValue.IsNullOrEmpty ? null : CacheValue.ToString();

        }

        public async Task SetDataAsync(string CacheKey, string CaheValue, TimeSpan TimeToLive)
        {
           await _database.StringSetAsync(CacheKey, CaheValue, TimeToLive);
        }
    }
}
