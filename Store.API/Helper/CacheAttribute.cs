using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Store.Service.CacheService;
using Store.Service.HandleResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheService= context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cachekey = GenerateCachekeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await _cacheService.GetCacheResponseAsync(cachekey);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType= "application/json",
                    StatusCode= 200
                };
                context.Result = contentResult;
                return;

            }
            var executedContext =await next();
            if (executedContext.Result is OkObjectResult response)
                await _cacheService.SetCacheResponseAsync(cachekey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }
        private string GenerateCachekeyFromRequest(HttpRequest request)
        {
            StringBuilder cachekey = new StringBuilder() ;
            cachekey.Append($"{request. Path}");

            foreach (var (key, value) in request.Query.OrderBy(x=> x.Key))
                cachekey.Append($"|{key}-{value}");
            return cachekey.ToString();
        }
    }
}
