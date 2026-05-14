using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contarct
{
    public interface ICacheRepository
    {
        // set data in cache 
        Task SetDataAsync(string CacheKey, string CaheValue , TimeSpan TimeToLive);

        // get data from cache
        Task<string?> GetDataAsync(string CacheKey);

    }
}
