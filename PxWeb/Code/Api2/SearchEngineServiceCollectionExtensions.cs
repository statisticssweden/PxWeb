using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Px.Search.Lucene;
using Px.Search;

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
            }

        }
    }
}
