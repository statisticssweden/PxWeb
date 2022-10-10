using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System;

namespace PxWeb
{
    public class CacheMiddleware
    {
        private readonly RequestDelegate _next;

        public CacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private async Task<string> readResponse(HttpContext httpContext)
        {
            using (var ms = new MemoryStream())
            {
                Stream originalStream = httpContext.Response.Body;
                httpContext.Response.Body = ms;
                await _next(httpContext);

                ms.Seek(0, SeekOrigin.Begin);
                string body = await new StreamReader(ms).ReadToEndAsync();
                ms.Seek(0, SeekOrigin.Begin);

                httpContext.Response.Body = originalStream;

                return body;
            }
        }

        public async Task Invoke(HttpContext httpContext, IPxCache cache)
        {
            HttpRequest request = httpContext.Request;

            string body = await new StreamReader(request.Body).ReadToEndAsync();

            // Get url
            string url = $"{request.Method}:{request.Scheme}://{request.Host.Value}{request.Path}{request.QueryString}";
            string key = $"{url}";
            if (request.Method == "POST" && body != "")
            {
                key += $":{body}";
            }

            string response;
            string? cached = cache.Get<string>(key);
            if (cached is null)
            {
                response = readResponse(httpContext).Result;
                // Checl if key exists, lock
                cache.Set(key, response);
            }
            else
            {
                response = cached;
            }

            httpContext.Response.ContentType = "application/json; charset = UTF-8";
            await httpContext.Response.WriteAsync(response);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheMiddleware>();
        }
    }
}
