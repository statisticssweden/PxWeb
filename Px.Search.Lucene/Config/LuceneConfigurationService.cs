using Microsoft.Extensions.Options;
using Px.Abstractions.Interfaces;
using System.Text;

namespace Px.Search.Lucene.Config
{
    public class LuceneConfigurationService : ILuceneConfigurationService
    {
        private readonly LuceneConfigurationOptions _configOptions;
        private readonly IPxHost _hostingEnvironment;

        public LuceneConfigurationService(IOptions<LuceneConfigurationOptions> configOptions, IPxHost hostingEnvironment)
        {
            _configOptions = configOptions.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public LuceneConfigurationOptions GetConfiguration()
        {
            return _configOptions;
        }

        /// <summary>
        /// Get path to the specified index directory 
        /// </summary>
        /// <returns>Physical path to Lucene index directory</returns>
        public string GetIndexDirectoryPath()
        {
            var luceneOptions = GetConfiguration();

            if (string.IsNullOrWhiteSpace(luceneOptions.IndexDirectory))
            {
                throw new Exception("Index directory not configured for Lucene index");
            }

            string indexDirectory = Path.Combine(_hostingEnvironment.RootPath, luceneOptions.IndexDirectory);

            if (Directory.Exists(indexDirectory))
            {
                StringBuilder dir = new StringBuilder(indexDirectory);

                dir.Append(@"/_INDEX/");

                return dir.ToString();
            }
            else
            {
                throw new Exception("Non existing index directory configured for Lucene index: " + indexDirectory);
            }
        }
    }
}
