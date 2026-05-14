using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Attrbuites
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _duration;

        public RedisCacheAttribute(int duration = 5) {
            _duration = duration;
        }
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, //  catch the request before it reaches the action method
            ActionExecutionDelegate next   // refere to the action that will be excuted after the fillter
                                          // can be refere to endpoint or to another fillter
            )
        {
            // get cache servies from DI container explicitly 
            var _CacheServies = context.HttpContext.RequestServices.GetRequiredService<ICacheServies>();

            // create cache key based on the request path and query string
            var CacheKey = CreateCacheKey(context.HttpContext.Request);

            // check if data exist in cache or not
            var CacheData = await _CacheServies.GetDataAsync(CacheKey);

            // if exist return data from cache and skip the action method
            if (CacheData is not null) {

                context.Result = new ContentResult() { 
                 
                    Content = CacheData,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,


                };

                return;
            }
            // if not exist execute the action method and store the data in cache
            var nextContext = await next.Invoke();
            if (nextContext.Result is OkObjectResult result) {

               await _CacheServies.SetDataAsync(CacheKey, 
                     result.Value!,
                     TimeSpan.FromMinutes(_duration)
               );
            
            }

 
        }



        private string CreateCacheKey(HttpRequest request) {

            StringBuilder Key = new StringBuilder();

            Key.Append(request.Path); //  add the request path to the key

            // loop on query string and ad to key 
            // api/product?brandid=2&typeid=1
            foreach(var item in request.Query.OrderBy(X => X.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");// api/product|brandid-2|typeid-1
            }

            return Key.ToString();

        }
    }



}
