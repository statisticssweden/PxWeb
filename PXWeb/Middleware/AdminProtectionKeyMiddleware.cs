using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using PxWeb.Config.Api2;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace PxWeb.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AdminProtectionKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AdminProtectionConfigurationOptions _adminProtectionConfigurationOptions;

        public AdminProtectionKeyMiddleware(RequestDelegate next, IAdminProtectionConfigurationService adminProtectionConfigurationService)
        {
            _next = next;
            _adminProtectionConfigurationOptions = adminProtectionConfigurationService.GetConfiguration();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string adminHeader = httpContext.Request.Headers["API_ADMIN_KEY"];
            bool match = _adminProtectionConfigurationOptions.AdminKey != null && _adminProtectionConfigurationOptions.AdminKey == adminHeader;
            if (!match)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            await _next(httpContext);


        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AdminProtectionKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminProtectionKey(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminProtectionKeyMiddleware>();
        }
    }
}
