using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface ICacheServies
    {
        // get data from cache
        Task<string?> GetDataAsync(string CacheKey);

        // set data in cache
        Task SetDataAsync(string CacheKey, object Cachevalue, TimeSpan TimeToLive);
    }
}
