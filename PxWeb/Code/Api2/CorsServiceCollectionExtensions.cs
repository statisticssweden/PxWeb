using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PxWeb.Code.Api2
{
    public static class CorsServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to handle CORS configuration
        /// </summary>
        public static bool ConfigurePxCORS(this IServiceCollection services, WebApplicationBuilder builder, ILogger logger)
        {
            bool corsEnbled = false;

            try
            {
                // Read configuration for CORS enabled
                bool.TryParse(builder.Configuration.GetSection("PxApiConfiguration:Cors:Enabled").Value.Trim(), out corsEnbled);
            }
            catch (System.Exception)
            {
                corsEnbled = false;
                logger.LogError("Could not read CORS Enabled configuration");
                return false;   
            }

            if (corsEnbled)
            {
                string[] origins = { "" };

                try
                {
                    // Read configuration for CORS origins
                    var originsConfig = builder.Configuration.GetSection("PxApiConfiguration:Cors:Origins").Value;
                    origins = originsConfig.Split(',', System.StringSplitOptions.TrimEntries);
                }
                catch (System.Exception)
                {
                    logger.LogError("Could not read CORS origins configuration");
                    return false;
                }

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

            return corsEnbled;
        }
    }
}
