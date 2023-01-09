using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{
    public class LuceneBackend : ISearchBackend
    {
        //TODO Add config settings if necessary for initializing the objects -> Lucene section at root level in appsettings.json
        //use the options pattern and DI


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
        /// <param name="database">database</param>
        /// <param name="language">language</param>
        /// <returns></returns>
        private string GetIndexDirectoryPath()
        {
            //TODO: Get index directory path from configuration in Lucene section
            string indexDirectory = "";


            
            if (Directory.Exists(indexDirectory))
            {
                StringBuilder dir = new StringBuilder(indexDirectory);

                dir.Append(@"\_INDEX\");
                //dir.Append(_language);

                return dir.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
