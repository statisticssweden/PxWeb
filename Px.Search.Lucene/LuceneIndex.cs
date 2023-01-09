using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{

    //TODO look at https://github.com/statisticssweden/Px.Search.Lucene/blob/main/Px.Search.Lucene/LuceneIndexer.cs for inspiration

    public class LuceneIndex : IIndex
    {
        private string _indexDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        public LuceneIndex(string indexDirectory)
        {
            _indexDirectory = indexDirectory;
        }
                public void AddEntry(string id, string label, DateTime updated, string[] tags, string category, string source)
        {
            throw new NotImplementedException();
        }

        public void BeginUpdate(string language)
        {
            throw new NotImplementedException();
        }

        public void BeginWrite(string language)
        {
            throw new NotImplementedException();
        }

        public void EndUpdate(string language)
        {
            throw new NotImplementedException();
        }

        public void EndWrite(string language)
        {
            throw new NotImplementedException();
        }

        public void RemoveEntry(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntry(string id, string label, DateTime updated, string[] tags, string category, string source)
        {
            throw new NotImplementedException();
        }
    }
}
