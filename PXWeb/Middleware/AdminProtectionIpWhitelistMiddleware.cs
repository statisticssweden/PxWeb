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
    public class AdminProtectionIpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AdminProtectionConfigurationOptions _adminProtectionConfigurationOptions;

        public AdminProtectionIpWhitelistMiddleware(RequestDelegate next, IAdminProtectionConfigurationService adminProtectionConfigurationService)
        {
            _next = next;
            _adminProtectionConfigurationOptions = adminProtectionConfigurationService.GetConfiguration();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            List<string> ipWhitelist = _adminProtectionConfigurationOptions.IpWhitelist;
            IPAddress? ip = httpContext.Connection.RemoteIpAddress;
            bool match = ipWhitelist.Where(x => IPAddress.Parse(x).Equals(ip)).Any();
            if (!match)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            await _next(httpContext);
           

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AdminProtectionIpWhitelistMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminProtectionIpWhitelist(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminProtectionIpWhitelistMiddleware>();
        }
    }
}
