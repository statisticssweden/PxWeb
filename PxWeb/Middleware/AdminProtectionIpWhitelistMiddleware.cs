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
        private readonly HashSet<string> _ipWhitelist = new HashSet<string>();

        public AdminProtectionIpWhitelistMiddleware(RequestDelegate next, IAdminProtectionConfigurationService adminProtectionConfigurationService)
        {
            _next = next;
            _adminProtectionConfigurationOptions = adminProtectionConfigurationService.GetConfiguration();
            List<string> ipWhitelist = _adminProtectionConfigurationOptions.IpWhitelist;
            foreach (string ip in ipWhitelist) _ipWhitelist.Add(ip);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IPAddress? ip = httpContext.Connection.RemoteIpAddress;
            bool match = ip != null && _ipWhitelist.Contains(ip.ToString());
            if (!match)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
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
