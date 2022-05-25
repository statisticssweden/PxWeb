using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PxWeb.Config.Api2;
using System.Collections.Generic;

namespace PxWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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


            // Get Cors configuration from appsettings.json
            bool corsEnbled = false;            
            bool.TryParse(builder.Configuration.GetSection("PxApiConfiguration:Cors:Enabled").Value.Trim(), out corsEnbled);
           
            if (corsEnbled) 
            {
                bool allowAnyOrigin = false;
                var withOriginsConfig = builder.Configuration.GetSection("PxApiConfiguration:Cors:Origins").Value;
                string withOrigins = string.Empty;

                if (withOriginsConfig.Trim() == "*")
                {
                    allowAnyOrigin = true;
                }
                else
                {
                    string result = string.Empty;
                    foreach (var origin in withOriginsConfig.Split(','))
                    {
                        result = result + "\"" + origin.Trim() + "\", ";
                    }
                    withOrigins = result.Remove(result.Length - 2);
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
                            policy.WithOrigins(withOrigins);
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

    }

}