using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZerdaLiveApi.Controllers;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";
        //private readonly ZerdaLiveContext _context ;      
        public ApiKeyAttribute()
        {
          
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };
                return;
            }
           
            var ApiKeys_List = UserRole.ApiList;
            var SearchResult = ApiKeys_List.Where(c => c.ApiKey1.Equals(extractedApiKey));
            if (!SearchResult.Any())
            {
                context.Result = new ContentResult()
                {
                   StatusCode = 401,
                   Content = "Api Key is not valid"
                };
                return;
            }
            await next();
        }
    }
}
