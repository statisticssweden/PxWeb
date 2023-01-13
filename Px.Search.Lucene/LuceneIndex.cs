using PCAxis.Paxiom;
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
        private string _indexDirectoryBase;
        private string _indexDirectoryCurrent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        public LuceneIndex(string indexDirectory)
        {
            _indexDirectoryBase = indexDirectory;
        }

        public void AddEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta)
        {
            throw new NotImplementedException();
        }

        public void BeginUpdate(string language)
        {
            throw new NotImplementedException();
        }

        public void BeginWrite(string language)
        {
            _indexDirectoryCurrent = Path.Combine(_indexDirectoryBase, language);
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

        public void UpdateEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta)
        {
            throw new NotImplementedException();
        }
    }
}
