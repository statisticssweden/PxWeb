using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Px.Search.Lucene;
using Px.Search;
using PxWeb.Config.Api2;
using Px.Search.Lucene.Config;

namespace PxWeb.Code.Api2
{
    public static class SearchEngineServiceCollectionExtensions
    {
        public static void AddPxSearchEngine(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var searchEngine = builder.Configuration.GetSection("PxApiConfiguration:SearchEngine");

            if (searchEngine.Value.ToUpper() == "LUCENE")
            {
                // Lucene search engine
                builder.Services.AddTransient<ISearchBackend, LuceneBackend>();

                // Add configuration
                builder.Services.Configure<LuceneConfigurationOptions>(builder.Configuration.GetSection("LuceneConfiguration"));
                builder.Services.AddTransient<ILuceneConfigurationService, LuceneConfigurationService>();
            }
            else
            {
                throw new System.Exception("No search engine configured for PxApi");
            }

        }
    }
}
