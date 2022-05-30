using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PxWeb.Config.Api2;
using System.Collections.Generic;

namespace PxWeb
{
    public class Program
    {
        private static ILogger<Program> _logger;

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

            // needed to load configuration from appsettings.json
            builder.Services.AddOptions();

            // needed to store rate limit counters and ip rules
            builder.Services.AddMemoryCache();

            //load general configuration from appsettings.json
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            builder.Services.AddInMemoryRateLimiting();

            // configuration (resolvers, counter key builders)
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Add configuration
            builder.Services.Configure<PxApiConfigurationOptions>(builder.Configuration.GetSection("PxApiConfiguration"));
            builder.Services.AddTransient<IPxApiConfigurationService, PxApiConfigurationService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Handle CORS configuration from appsettings.json
            bool corsEnbled = IsCORSEnabled(builder);        
            
            if (corsEnbled) 
            {
                string[] origins = GetCORSOrigins(builder);
                bool allowAnyOrigin = false;

                if (origins[0] == "*" || origins[0] == "")
                {
                    allowAnyOrigin = true;
                }

                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                    policy =>
                    {
                        if (allowAnyOrigin)
                        {
                            policy.AllowAnyOrigin();
                        }
                        else
                        {
                            policy.WithOrigins(origins);
                        }
                    });
                });
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            if (corsEnbled)
            {
                app.UseCors();
            }

            app.UseAuthorization();

            app.MapControllers();

            if (!app.Environment.IsDevelopment())
            {
                app.UseIpRateLimiting();
            }

            app.Run();
        }

        /// <summary>
        /// Check configuration file if CORS is enabled
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static bool IsCORSEnabled(WebApplicationBuilder builder)
        {
            bool corsEnbled = false;

            try
            {
                bool.TryParse(builder.Configuration.GetSection("PxApiConfiguration:Cors:Enabled").Value.Trim(), out corsEnbled);
            }
            catch (System.Exception)
            {
                corsEnbled = false;
                _logger.LogError("Could not read CORS Enabled configuration");
            }

            return corsEnbled;
        }

        /// <summary>
        /// Get CORS origins from configuration file
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static string[] GetCORSOrigins(WebApplicationBuilder builder)
        {
            string[] origins = { "" };

            try
            {
                var originsConfig = builder.Configuration.GetSection("PxApiConfiguration:Cors:Origins").Value;
                origins = originsConfig.Split(',', System.StringSplitOptions.TrimEntries);
            }
            catch (System.Exception)
            {
                _logger.LogError("Could not read CORS origins configuration");
            }

            return origins;
        }

    }

}