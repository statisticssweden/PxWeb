using Microsoft.AspNetCore.Hosting;
using Px.Search.Lucene.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{
    public class LuceneBackend : ISearchBackend
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILuceneConfigurationService _luceneConfigurationService;

        public LuceneBackend(IWebHostEnvironment hostingEnvironment, ILuceneConfigurationService luceneConfigurationService)
        {
            _hostingEnvironment = hostingEnvironment;   
            _luceneConfigurationService = luceneConfigurationService;   
        }

        public IIndex GetIndex()
        {
            string path = GetIndexDirectoryPath();
            return new LuceneIndex(path);
        }

        public ISearcher GetSearcher()
        {
            //TODO Create and initialize an instance of LuceneSearcher
            throw new NotImplementedException();
        }

 
        /// <summary>
        /// Get path to the specified index directory 
        /// </summary>
        /// <returns>Physical path to Lucene index directory</returns>
        private string GetIndexDirectoryPath()
        {
            var luceneOptions = _luceneConfigurationService.GetConfiguration();

            if (string.IsNullOrWhiteSpace(luceneOptions.IndexDirectory))
            {
                throw new Exception("Index directory not configured for Lucene index");
            }

            string indexDirectory = Path.Combine(_hostingEnvironment.WebRootPath, luceneOptions.IndexDirectory);

            if (Directory.Exists(indexDirectory))
            {
                StringBuilder dir = new StringBuilder(indexDirectory);

                dir.Append(@"\_INDEX\");

                return dir.ToString();
            }
            else
            {
                throw new Exception("Non existing index directory configured for Lucene index: " + indexDirectory);
            }
        }
    }
}
