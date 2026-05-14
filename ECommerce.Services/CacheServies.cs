using ECommerce.Domain.Contarct;
using ECommerce.ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class CacheServies : ICacheServies
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheServies(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetDataAsync(string CacheKey)
        {
            return await _cacheRepository.GetDataAsync(CacheKey);
        }

        public async Task SetDataAsync(string CacheKey, object Cachevalue, TimeSpan TimeToLive)
        {

            var CacheData = JsonSerializer.Serialize(Cachevalue); //  convert object to string to store in cache
            await _cacheRepository.SetDataAsync(CacheKey, CacheData, TimeToLive);
        }
    }
}
